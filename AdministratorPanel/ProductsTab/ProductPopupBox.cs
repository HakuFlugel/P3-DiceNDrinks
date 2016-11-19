﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;

namespace AdministratorPanel {
    class ProductPopupBox : FancyPopupBox {
        NiceTextBox productName = new NiceTextBox() {
            Width = 200,
            waterMark = "Product Name",
        };

        ComboBox categoryName = new ComboBox() {
            Width = 200,
            Text = "Category Name",
            Name = "Category Name",
        };

        ComboBox sectionName = new ComboBox() {
            Width = 200,
            Text = "Section Name",
        };

        Panel productImage = new Panel() {
            Width = 150,
            Height = 150,
            BackgroundImage = Image.FromFile("images/_default.png"),
            Dock = DockStyle.Top,
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        DataGridView priceElements = new DataGridView() {
            Dock = DockStyle.Fill,
            Height = 280,
            RowHeadersVisible = false,
            ScrollBars = ScrollBars.Vertical,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
        };
        

        private ProductsTab productTab;
        private ProductItem productItem;
        private DataTable dataTable = new DataTable();
        private Image image;
        private string imageName = "_default.png";

        public ProductPopupBox() { }

        public ProductPopupBox(ProductsTab productTab, ProductItem productItem = null) {
            this.productTab = productTab;
            this.productItem = productItem;

            Focus();

            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));

            if (this.productItem != null) {
                productName.Text = this.productItem.product.name;
                categoryName.Text = this.productItem.product.category;
                sectionName.Text = this.productItem.product.section;
                imageName = this.productItem.product.image;

                loadPriceElements(productItem.product.PriceElements);

                try {
                    image = Image.FromFile("images/" + productItem.product.image);
                    productImage.BackgroundImage = image;

                } catch (Exception e) {
                    MessageBox.Show(e.Message);
                }

            } else {
                Controls.Find("delete", true).First().Enabled = false;
            }
            priceElements.DataSource = dataTable;
            update();
        }


        private void update() {
            categoryName.Items.Clear();
            // update dropdown category
            foreach (ProductCategory item in productTab.productCategories) {
                categoryName.Items.Add(item.name);
            }

            categoryName.SelectedValueChanged += (s, e) => {
                sectionName.Items.Clear();
                if (categoryName.Text != "Category Name" && categoryName.Text != "") {
                    ProductCategory cat = productTab.productCategories.First(o => o.name == categoryName.Text);
                    sectionName.Items.AddRange(cat.sections.ToArray());
                    sectionName.Text = "Section Name";
                    //test
                    Console.WriteLine("sections");
                    foreach (var item in cat.sections) {
                        Console.WriteLine(item);
                    }
                }
                Console.WriteLine("catname = " + categoryName.Text);
            };
            // update dropdown section

            

        }

        protected override Control CreateControls() {

            // outer panel
            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            // inner panels
            TableLayoutPanel rightPanel = new TableLayoutPanel();
            rightPanel.ColumnCount = 1;
            rightPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rightPanel.AutoSize = true;

            TableLayoutPanel leftPanel = new TableLayoutPanel();
            leftPanel.ColumnCount = 1;
            leftPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            leftPanel.AutoSize = true;

            // priceElements list display                 
            priceElements.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            // Add button click event
            productImage.Click += (s, e) => {
                //TODO: might be wr0ng

                // open file dialog for image selection
                OpenFileDialog ofd = new OpenFileDialog();
                var imageCodeInfo = ImageCodecInfo.GetImageDecoders();
                StringBuilder sb = new StringBuilder();

                foreach (ImageCodecInfo item in imageCodeInfo) {
                    sb.Append(item.FilenameExtension);
                    sb.Append(";");
                }

                ofd.Title = "Open Image";
                ofd.Filter = "Image Files | " + sb.ToString() ;

                if (ofd.ShowDialog() == DialogResult.OK) {

                    if (File.Exists("images/" + ofd.SafeFileName)) {
                        MessageBox.Show("imagename already exist");
                    }

                    try {
                        imageName =  ofd.SafeFileName; // name
                        image = Image.FromFile( ofd.FileName); // path + name
                        productImage.BackgroundImage = image; 
                        
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            };

            leftPanel.Controls.Add(productName);
            leftPanel.Controls.Add(categoryName);
            leftPanel.Controls.Add(sectionName);
            leftPanel.Controls.Add(productImage);

            rightPanel.Controls.Add(priceElements);

            header.Controls.Add(leftPanel);
            header.Controls.Add(rightPanel);
            
            return header;
        }


        private void loadPriceElements(List<PriceElement> priceElements) {
            
            if (priceElements.Count > 0) {

                foreach (var item in priceElements) {

                    dataTable.Rows.Add(item.name, item.price);
                }
            }
        }
        
        private List<PriceElement> SavePriceElemets() {
            List<PriceElement> pr = new List<PriceElement>();

            int index = dataTable.Rows.Count;

            for (int first = 0; first < index; first++) {
                PriceElement priceElement = new PriceElement();
                priceElement.name = dataTable.Rows[first][0].ToString(); // string(name)
                priceElement.price = decimal.Parse(dataTable.Rows[first][1].ToString()); // decimal(price)
                pr.Add(priceElement);
            }

            return pr;
        }

        protected override void delete(object sender, EventArgs e) {
            productTab.productList.Remove(productItem.product);
        }

        protected override void save(object sender, EventArgs e) {

            Directory.CreateDirectory("images");

            if (image == null) {
                MessageBox.Show("No image in product ");
                image = productImage.BackgroundImage;
            } else {
                Console.WriteLine(imageName);
                if (!File.Exists("images/" + imageName)) {
                    image.Save("images/" + imageName);
                }
            }


            if (productItem == null) {
                Product tempProduct = new Product();

                tempProduct.name = productName.Text;
                tempProduct.PriceElements = SavePriceElemets();
                tempProduct.category = categoryName.Text;
                tempProduct.section = sectionName.Text;
                tempProduct.image = imageName;

                productItem = new ProductItem(tempProduct,productTab);

                productTab.productList.Add(productItem.product);
                productTab.AddProductItem(productItem);

            } else if (categoryName.Text != productItem.product.category || sectionName.Text != productItem.product.section) {
                productItem.Parent.Controls.Remove(productItem);
                productTab.AddProductItem(productItem);

                productItem.product.name = productName.Text;
                productItem.product.PriceElements = SavePriceElemets();
                productItem.product.category = categoryName.Text;
                productItem.product.section = sectionName.Text;
                productItem.product.image = imageName;
            }

            Close();
        }
    }
}