﻿using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Shared;
using System.Drawing;
using System;

namespace AdministratorPanel {
    public class ProductItem : NiceButton {

        private Panel imagePanel = new Panel() {
            Name = "Image",
            Height = 86,
            BackgroundImageLayout = ImageLayout.Zoom,
            BackColor = Color.Black
        };

        private Label productName = new Label() {
            Name = "ProductName",
            Dock = DockStyle.Fill
        };

        private TableLayoutPanel priceTable = new TableLayoutPanel() {
            Name = "PriceTableName",
            ColumnCount = 2,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows
        };

        private int sizeX = 192;
        private int sizeY = 192;
        private Image image;
        public Product product;
        private ProductsTab productTab;

        public ProductItem() { }

        public ProductItem(Product product, ProductsTab productTab) {
            this.productTab = productTab;
            this.product = product;
            if (product.name != null && product.section != null && product.category != null) {
                Update(product);
            }

            bgColor = Color.LightGray;
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize; 
            RowCount = 3;
            ColumnCount = 1;

            CreadControls();
        }

        private void CreadControls() {
            //Product Image 
            imagePanel.Width = ClientSize.Width;
            imagePanel.BackgroundImage = image;
            Controls.Add(imagePanel);

            //Product Name
            productName.Text = product.name;
            Controls.Add(productName);

            //Product Prices

            Controls.Add(priceElementsLoader(product, priceTable));
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
                string path = "images/products/";
                string fileToLoad = path + product.image;

                if (product.image == null) {
                    fileToLoad = path + "_default.png";
                }
                image = Image.FromFile(fileToLoad);
            } catch (Exception) {
                if (product.image != null) {
                    //NiceMessageBox.Show($"image : {product.image} not found");
                    Console.WriteLine($"Error: image \"{product.image}\" not found");

                    product.image = null;
                    Update(product);
                }
            }
            this.Height = sizeY;
            this.Width = sizeX;
        }

        protected override void OnClick(EventArgs e)
        {
            new ProductPopupBox(productTab, this);
            //base.OnClick(e);
        }
    }
}
