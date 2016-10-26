using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Drawing;

namespace AdministratorPanel {
    class ProductItem : Button{

        private int sizeX = 160;
        private int sizeY = 160;
        TableLayoutPanel tb = new TableLayoutPanel();
        

        private string name;
        private Image image = Image.FromFile("C:/Users/Bruger/Desktop/Red.png");
        private List<PriceElement> priceElements;

        public ProductItem(Product p) {
            Update();
            //table();
            this.name = p.name;
            //this.Text = Information();
            this.priceElements = p.PriceElements;
            this.Image = image;
            this.TextImageRelation = TextImageRelation.ImageAboveText;
            
        }

        private string Information() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            //sb.
            foreach (var item in priceElements) {
                sb.AppendLine(item.name + "Price: " + item.price);
            }
            
            return sb.ToString();

            
        }

        private void table() {
            tb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tb.Dock = DockStyle.Fill;
            tb.RowCount = 2;
            tb.ColumnCount = 2;
            tb.Name = "Information";
            tb.BackColor = Color.Transparent;
            tb.Controls.Add(new Button { Height = 20, Width = 20, Text = "1" });
            tb.Controls.Add(new Button { Height = 20, Width = 20, Text = "2" });
            tb.Controls.Add(new Button { Height = 20, Width = 20, Text = "3" });
            tb.Controls.Add(new Button { Height = 20, Width = 20, Text = "4" });
            Controls.Add(tb);

        }

        private void Update(){
            this.Height = sizeY;
            this.Width = sizeX;
            
        }
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            Console.WriteLine(this);
        }

    }
}
