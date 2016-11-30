using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Shared;
using System.Drawing;
using System;
using System.IO;

namespace AdministratorPanel {
    public class ProductItem : NiceButton {

        private int sizeX = 192;
        private int sizeY = 192;
        private Image image;
        public Product product;
        private ProductsTab productTab;

        public ProductItem() {

        }

        public ProductItem(Product product, ProductsTab productTab) {
            this.productTab = productTab;
            this.product = product;
            if (product.name != null && product.section != null && product.category != null) {
                Update(product);
            }

            bgColor = Color.LightGray;

            GrowStyle = TableLayoutPanelGrowStyle.FixedSize; // note skal måske ændres
            RowCount = 3;
            ColumnCount = 1;
            
            //1
            Panel pn = new Panel();
            pn.Name = "Image";
            pn.Height = 64;
            pn.Width = ClientSize.Width;
            
            pn.BackgroundImage = image;
            pn.BackgroundImageLayout = ImageLayout.Zoom;
            pn.BackColor = Color.Black;
            Controls.Add(pn);

            //2
            Label productName = new Label();
            productName.Name = "ProductName";
            productName.Dock = DockStyle.Fill;
            productName.Text = product.name;
            Controls.Add(productName);

            //3
            TableLayoutPanel priceTable = new TableLayoutPanel();
            priceTable.Name = "PriceTableName";
            priceTable.AutoSize = true;
            priceTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            priceTable.Dock = DockStyle.Fill;
            priceTable.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            priceTable.ColumnCount = 2;

            Controls.Add(priceElementsLoader(product,priceTable));

        }

        private TableLayoutPanel priceElementsLoader(Product product, TableLayoutPanel tableLayOutPanel) {
            
            List<Label> lList = new List<Label>();
            
            foreach (var item in product.PriceElements) {
                StringBuilder prefix = new StringBuilder();
                StringBuilder price = new StringBuilder();

                Label preLabel = new Label();
                Label priceLabel = new Label();

                prefix.Append(item.name);
                preLabel.Name = "PriceElementName";
                preLabel.Text = prefix.ToString();
                lList.Add(preLabel);
               
                price.Append(item.price);

                priceLabel.Name = "PriceElementPrice";
                price.Append(" kr.");
                priceLabel.Text = price.ToString();
                lList.Add(priceLabel);
            }

            tableLayOutPanel.Controls.AddRange(lList.ToArray());
            return tableLayOutPanel;
        }

        private void Update(Product product){

            try {
                string path = "images/";
                string fileToLoad = path + product.image;

                if (product.image == null) {
                    fileToLoad = path + "_default.png";
                }

                image = Image.FromFile(fileToLoad);
            } catch (Exception) {
                if (product.image != null) {
                    MessageBox.Show($"image : {product.image} not found");

                    product.image = null;
                    Update(product);

                }
            }

            this.Height = sizeY;
            this.Width = sizeX;
        }

        protected override void OnClick(EventArgs e) {
            ProductPopupBox p = new ProductPopupBox(productTab, this);
            p.Show();
            base.OnClick(e);

        }
    }
}
