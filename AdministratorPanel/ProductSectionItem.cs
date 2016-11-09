using System.Collections.Generic;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    class ProductSectionItem : TableLayoutPanel{
        private FlowLayoutPanel flowPanel = new FlowLayoutPanel();
        private ProductSection section;

        public ProductSectionItem(ProductSection section) {
            this.section = section;
            Dock = DockStyle.Fill;
            AutoSize = true;

            ColumnCount = 1;
            RowCount = 2;
            //Text = section.name;

            //Title for the section
            Label title = new Label();
            title.Text = section.name;
            title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            title.Dock = DockStyle.Fill;
            title.Font = new System.Drawing.Font(title.Font.Name, title.Font.Size * 4);
            title.AutoSize = true;

            Controls.Add(title);

            // section content
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoSize = true;
            Controls.Add(flowPanel);
        }

        public void makeItems(List<Product> products) {
            
            flowPanel.Controls.Clear();

            foreach (var item in products) {

                ProductItem product = new ProductItem(item);
                flowPanel.Controls.Add(product);
            }
        }
    }
}
