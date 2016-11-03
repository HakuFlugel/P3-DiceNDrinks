using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using System;

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

        public override void Save() {
            //throw new NotImplementedException();
        }

        public override void Load() {
            //throw new NotImplementedException();
        }
    }
}
