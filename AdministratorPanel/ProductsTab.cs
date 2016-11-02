using System.Windows.Forms;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class ProductsTab : AdminTabPage {

        public List<Product> products;

        public ProductsTab() {
            Text = "Products";
            TabControl tb = new TabControl();
            tb.Dock = DockStyle.Fill;

            tb.Controls.Add(new ProductCategoryTab("test"));
            Controls.Add(tb);

        }
    }
}
