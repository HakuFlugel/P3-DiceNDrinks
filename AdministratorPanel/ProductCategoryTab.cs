using System.Collections.Generic;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class ProductCategoryTab : TabPage{
        private ProductCategory category;
        private TableLayoutPanel table = new TableLayoutPanel();

        public ProductCategoryTab(ProductCategory productCategory) {
            this.category = productCategory;
            Text = productCategory.name;
            AutoScroll = true;
            Dock = DockStyle.Fill;

            table.Dock = DockStyle.Top;
            table.ColumnCount = 1;
            table.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            table.AutoSize = true;
            
            makeItems(productCategory.sections); // make sections

            Controls.Add(table);
        }

        public void makeItems(List<ProductSection> productSections) {

            table.Controls.Clear();

            foreach (var item in productSections) {

                ProductSectionItem section = new ProductSectionItem(item);
                section.makeItems(item.products); // makes all productItems in sections
                table.Controls.Add(section);
            }
        }
    }
}
