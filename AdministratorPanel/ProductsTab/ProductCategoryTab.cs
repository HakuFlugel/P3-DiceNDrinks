using System.Collections.Generic;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class ProductCategoryTab : TabPage{
        public ProductCategory category;
        public TableLayoutPanel table = new TableLayoutPanel();

        public ProductCategoryTab(ProductCategory productCategory) {
            this.category = productCategory;
            Name = productCategory.name;
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

        public void makeItems(List<string> productSections) {

            table.Controls.Clear();

            foreach (var item in productSections) {

                ProductSectionItem section = new ProductSectionItem(item);
               
                table.Controls.Add(section);
            }
        }
    }
}
