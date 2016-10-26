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

        public ProductCategoryTab() {
            Name = "Products";

            FlowLayout();
           
        }

        public void FlowLayout() {

            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;

            Product p1 = new Product();
            p1.name = "derp";
            Product p2 = new Product();
            p2.name = "herp";

            Product p3 = new Product();
            p3.name = "skov";

            List<Product> pl = new List<Product>();
            pl.Add(p1);
            pl.Add(p2);
            pl.Add(p3);
            pl.Add(p1);
            pl.Add(p2);
            pl.Add(p3);
            pl.Add(p1);
            pl.Add(p2);
            pl.Add(p3);

            foreach (var item in pl) {
                flowPanel.Controls.Add(new ProductItem(item));
            }

            Controls.Add(flowPanel);
        }

    }
}
