using System.Windows.Forms;

namespace AdministratorPanel {
    public class ProductsTab : TabPage {
        public ProductsTab() {
            Text = "Products";
            TabControl tb = new TabControl();
            tb.Dock = DockStyle.Fill;

            tb.Controls.Add(new ProductCategoryTab("test"));
            Controls.Add(tb);

        }
    }
}
