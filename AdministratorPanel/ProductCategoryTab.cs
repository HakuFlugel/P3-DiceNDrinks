using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;



namespace AdministratorPanel {
    public class ProductCategoryTab : TabPage{

        //List<Objects> products = new List<Objects>();

        public ProductCategoryTab(string name) {
            Name = name;
            FlowLayout(); 
        }

        public void FlowLayout() {

            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;

            PriceElement pr = new PriceElement();
            pr.name = "Text for product";
            pr.price = 69;

            List<PriceElement> prl = new List<PriceElement>();
            prl.Add(pr);
            prl.Add(pr);
            prl.Add(pr);
            prl.Add(pr);

            Product p1 = new Product();
            p1.image = "images/Red.png";
            p1.name = "Name of product";
            p1.PriceElements = prl;

            List<Product> pl = new List<Product>();
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);
            pl.Add(p1);

            foreach (var item in pl) {
                flowPanel.Controls.Add(new ProductItem(item));
            }

            Controls.Add(flowPanel);
        }

    }
}
