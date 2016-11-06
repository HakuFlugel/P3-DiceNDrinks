using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Globalization;

namespace AdministratorPanel {
    class ProductPopupBox : FancyPopupBox {
        NiceTextBox productName = new NiceTextBox() {
            Width = 200,
            waterMark = "Product Name",
            Margin = new Padding(4, 10, 20, 10)
        };
        NiceTextBox prodcutDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Product Description",
            Multiline = true,
            Margin = new Padding(4, 0, 20, 10)
        };
        NiceTextBox productImage = new NiceTextBox() {
            Width = 100,
            Height = 100,
            Multiline = true,
            Margin = new Padding(4, 0, 20, 10)
        };


        private List<Buttom> makeItems() {
            List<Buttom> items = new List<Buttom>();
            foreach (var item in product.PriceElements) {
                Label label = new Label() { Text = item.name + "  " + item.price };
                Buttom priceElementItem = new Buttom();
                priceElementItem.Controls.Add(label);
                items.Add(priceElementItem);
            }
            return items;
        }

        private ProductsTab eventsTab;
        private Product product;
        
        public ProductPopupBox() {
        }

        public ProductPopupBox(ProductsTab eventsTab, Product product = null) {
            this.eventsTab = eventsTab;
            this.product = product;

            if (this.product != null) {
                productName.Text = this.product.name;
                productImage.Text = this.product.image;

            } else {
                Controls.Find("delete", true).First().Enabled = false;
            }
        }

        protected override Control CreateControls() {

            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            TableLayoutPanel rightPanel = new TableLayoutPanel();
            rightPanel.ColumnCount = 1;
            rightPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rightPanel.Dock = DockStyle.Fill;

            TableLayoutPanel leftPanel = new TableLayoutPanel();
            leftPanel.ColumnCount = 1;
            leftPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            leftPanel.Dock = DockStyle.Fill;

            TableLayoutPanel priceElements = new TableLayoutPanel();
            priceElements.ColumnCount = 1;
            priceElements.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            priceElements.Width = 100;
            priceElements.Height = 100;
            priceElements.AutoScroll = true;

            if (product != null) {                                          //Matias "Gør det altid på en dum måde" 
                priceElements.Controls.AddRange(makeItems().ToArray());
            }

            rightPanel.Controls.Add(productName);
            rightPanel.Controls.Add(priceElements);

            leftPanel.Controls.Add(prodcutDescription);

            header.Controls.Add(rightPanel);
            header.Controls.Add(leftPanel);
            return header;
        }

                                                                        // shana "Jeg skal nok lave det!"
        protected override void delete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        protected override void save(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}
