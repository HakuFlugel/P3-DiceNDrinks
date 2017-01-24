using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Shared;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;

namespace AdministratorPanel {
     public class ProductPopupBox : FancyPopupBox {
        private NiceTextBox productNameBox = new NiceTextBox() {
            Width = 200,
            waterMark = "Product Name",
        };

        private ComboBox categoryNameDropDownBoxx = new ComboBox() {
            Width = 200,
            Text = "Category Name",
            Name = "Category Name",
        };

        private ComboBox sectionNameDropDownBox = new ComboBox() {
            Width = 200,
            Text = "Section Name",
        };

        private Panel productImagePanel = new Panel() {
            Width = 150,
            Height = 150,
            BackgroundImage = Image.FromFile("images/_default.png"),
            Dock = DockStyle.Top,
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        private DataGridView priceElements = new DataGridView() {
            Dock = DockStyle.Fill,
            Height = 280,
            RowHeadersVisible = false,
            ScrollBars = ScrollBars.Vertical,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
        };

        TableLayoutPanel headerTableLayoutPanel = new TableLayoutPanel() {
            RowCount = 1,
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        TableLayoutPanel innerRightTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        TableLayoutPanel innerLeftTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        private ProductsTab productTab;
        private ProductItem productItem;
        private DataTable dataTable = new DataTable();  // source for priceElement box
        private Image image;
        private string imageName = "_default.png";      // default image

        public ProductPopupBox(ProductsTab productTab, ProductItem productItem = null) {
            //tab name
            Text = "Product";
            Name = (productItem != null) ? productItem.product.name : "New product";
            Console.WriteLine(Name);

            Focus();

            this.productTab = productTab;
            this.productItem = productItem;


            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));

            if (this.productItem != null) {
                productNameBox.Text = this.productItem.product.name;
                categoryNameDropDownBoxx.Text = this.productItem.product.category;
                sectionNameDropDownBox.Text = this.productItem.product.section;
                imageName = this.productItem.product.image;

                loadPriceElements(productItem.product.PriceElements);

                if (productItem.product.image != null) {
                    try {
                        image = Image.FromFile("images/products/" + productItem.product.image);
                        productImagePanel.BackgroundImage = image;

                    } catch (Exception e) {
                        NiceMessageBox.Show(e.Message);
                    }
                }

            } else {
                Controls.Find("delete", true).First().Enabled = false;
            }
            priceElements.DataSource = dataTable;
            SubscriptionController();
            update();
        }

        private void SubscriptionController() {
            categoryNameDropDownBoxx.SelectedValueChanged += (s, e) => {
                sectionNameDropDownBox.Items.Clear();
                if (categoryNameDropDownBoxx.Text != "Category Name" && categoryNameDropDownBoxx.Text != "") {
                    ProductCategory cat = productTab.productCategories.First(o => o.name == categoryNameDropDownBoxx.Text);
                    sectionNameDropDownBox.Items.AddRange(cat.sections.ToArray());
                    sectionNameDropDownBox.Text = "Section Name";
                }
                hasBeenChanged = (productItem != null) ? productItem.product.category != categoryNameDropDownBoxx.Text ? true : false : true;
            };
            priceElements.CellValueChanged += (s, e) => { hasBeenChanged = true; };
            productNameBox.TextChanged += (s, e) => { hasBeenChanged = (productItem != null) ? productItem.product.name != productNameBox.Text ? true : false : true; };
            sectionNameDropDownBox.SelectedValueChanged += (s, e) => { hasBeenChanged = (productItem != null) ? productItem.product.section != sectionNameDropDownBox.Text ? true : false : true; };
            productImagePanel.BackgroundImageChanged += (s, e) => { hasBeenChanged = true; };
        }

        private void update() {
            categoryNameDropDownBoxx.Items.Clear();
            // update dropdowns
            foreach (ProductCategory item in productTab.productCategories) {
                categoryNameDropDownBoxx.Items.Add(item.name);
            }
        }

        protected override Control CreateControls() {

            Controls.Add(headerTableLayoutPanel);

            // priceElements list display                 
            priceElements.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Add button click event
            productImagePanel.Click += (s, e) => {

                // Open file dialog for image selection
                OpenFileDialog ofd = new OpenFileDialog();
                var imageCodeInfo = ImageCodecInfo.GetImageDecoders();
                StringBuilder sB = new StringBuilder();

                foreach (ImageCodecInfo item in imageCodeInfo) {
                    sB.Append(item.FilenameExtension);
                    sB.Append(";");
                }

                ofd.Title = "Open Image";
                ofd.Filter = "Image Files | " + sB.ToString() ;

                if (ofd.ShowDialog() == DialogResult.OK) {

                    if (File.Exists("images/products/" + ofd.SafeFileName)) {
                        NiceMessageBox.Show("imagename already exist");
                    }

                    try {
                        imageName =  ofd.SafeFileName; // name
                        image = Image.FromFile( ofd.FileName); // path + name
                        productImagePanel.BackgroundImage = image;

                    } catch (Exception ex) {
                        NiceMessageBox.Show(ex.Message);
                    }
                }
            };

            innerLeftTableLayoutPanel.Controls.Add(productNameBox);
            innerLeftTableLayoutPanel.Controls.Add(categoryNameDropDownBoxx);
            innerLeftTableLayoutPanel.Controls.Add(sectionNameDropDownBox);
            innerLeftTableLayoutPanel.Controls.Add(productImagePanel);

            innerRightTableLayoutPanel.Controls.Add(priceElements);

            headerTableLayoutPanel.Controls.Add(innerLeftTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(innerRightTableLayoutPanel);

            return headerTableLayoutPanel;
        }

        private void loadPriceElements(List<PriceElement> priceElements) {

            if (priceElements.Count > 0) {
                foreach (var item in priceElements) {

                    dataTable.Rows.Add(item.name, item.price.ToString());
                }
            }
        }

        private List<PriceElement> SavePriceElemets() {
            List<PriceElement> priceElementList = new List<PriceElement>();

            int index = dataTable.Rows.Count;

            for (int row = 0; row < index; row++) {

                if (dataTable.Rows[row][0].ToString() == "" && dataTable.Rows[row][1].ToString() == "") {
                    continue;
                }


                bool invalidprice = false;
                decimal price = 0;
                try {
                    price = decimal.Parse(dataTable.Rows[row][1].ToString());
                } catch (Exception) {
                    invalidprice = true;
                }
                if ((dataTable.Rows[row][0].ToString() != "" && (dataTable.Rows[row][1].ToString() == "")||invalidprice)) {
                    NiceMessageBox.Show($"Invalid price on row {row+1}.\nThe product was not saved");
                    return null;
                }

                PriceElement priceElement = new PriceElement();
                priceElement.name = dataTable.Rows[row][0].ToString(); // string(name)
                priceElement.price = price;//decimal.Parse(dataTable.Rows[row][1].ToString()); // decimal(price)
                priceElementList.Add(priceElement);
            }
            return priceElementList;
        }

        protected override void delete(object sender, EventArgs e) {
            ProductCategoryTab categoryTab;
            ProductSectionItem productSectionItem;
            var categoryFound = productTab.tabControl.Controls.Find(productItem.product.category, true);
            categoryTab = categoryFound.First() as ProductCategoryTab;

            var sectionFound = categoryTab.table.Controls.Find(productItem.product.section, false);
            productSectionItem = sectionFound.First() as ProductSectionItem;

            ProductCategory productCategory = productTab.productCategories.Where(x => x.name == productItem.product.category).First();

            string response = ServerConnection.sendRequest("/submitProduct.aspx",
                new NameValueCollection() {
                    {"Action", "delete"},
                    {"Product", JsonConvert.SerializeObject(productItem.product)}
                }
            );

            Console.WriteLine(response);

            if (response != "deleted")
            {
                return;
            }

            productTab.productList.Remove(productItem.product);                    // removes product
            productSectionItem.headerFlowLayoutPanel.Controls.Remove(productItem); // removes product item for msection

            Console.WriteLine(productSectionItem.headerFlowLayoutPanel.Controls.Count);
            if (productSectionItem.headerFlowLayoutPanel.Controls.Count == 0) {
                productCategory.sections.Remove(productItem.product.section);
                categoryTab.table.Controls.Remove(productSectionItem);
            }


            if (categoryTab.table.Controls.Count == 0) {
                productTab.productCategories.Remove(productCategory);
                productTab.tabControl.Controls.Remove(categoryTab);
            }

            Close();

            productTab.Update();
        }

        protected override void save(object sender, EventArgs e) {
            // Image save
            Directory.CreateDirectory("images/products/");

            if (image == null) {
                if (DialogResult.OK != NiceMessageBox.Show("No image in product. Do you still want to save", "No Images", MessageBoxButtons.OKCancel)) {
                    return;
                }
                image = productImagePanel.BackgroundImage;
            } else {
                if (!File.Exists("images/products/" + imageName)) {
                    image.Save("images/products/" + imageName, ImageFormat.Png);
                }
            }
            // convert priceelements to saves
            List<PriceElement> listOfPriceElements = SavePriceElemets();
            if (listOfPriceElements == null) {
                NiceMessageBox.Show("No price set");
                return;
            }

            //make new productSave
            if (productItem == null) {
                Product product = new Product();

                product.name = productNameBox.Text;
                product.PriceElements = listOfPriceElements;
                product.category = categoryNameDropDownBoxx.Text;
                product.section = sectionNameDropDownBox.Text;
                product.image = imageName;

                string response2 = ServerConnection.sendRequest("/submitProduct.aspx",
                    new NameValueCollection() {
                        {"Action", "add"},
                        {"Product", JsonConvert.SerializeObject(product)},
                        {"Image", ImageHelper.imageToBase64(image)}
                    }
                );
                Console.WriteLine(response2);

                if (response2.Split(' ')[0] != "added")
                {
                    return;
                }

                int.TryParse(response2.Split(' ')[1], out product.id);

                productItem = new ProductItem(product, productTab);

                productTab.productList.Add(productItem.product);
                productTab.AddProductItem(productItem);

                Close();
                return;

            } // override a product
            else if (categoryNameDropDownBoxx.Text != productItem.product.category || sectionNameDropDownBox.Text != productItem.product.section) {
                productItem.Parent.Controls.Remove(productItem);
                productTab.AddProductItem(productItem);
            } 
            // save content
            productItem.product.name = productNameBox.Text;
            productItem.product.PriceElements = listOfPriceElements;
            productItem.product.category = categoryNameDropDownBoxx.Text;
            productItem.product.section = sectionNameDropDownBox.Text;
            productItem.product.image = imageName;

            try {
                string response = ServerConnection.sendRequest("/submitProduct.aspx",
                new NameValueCollection() {
                    {"Action", "update"},
                    {"Product", JsonConvert.SerializeObject(productItem.product)},
                    {"Image", ImageHelper.imageToBase64(image)}
                }
            );
                if (response.StartsWith("exception")) {
                    throw new Exception(response);
                }
                Console.WriteLine(response);

                if (response != "updated") {
                    return;
                }
            }
            catch (Exception) {
                NiceMessageBox.Show("Failed to save to the server, changes will be lost if this window is closed", "Server connection problem");
                return;
            }

            productTab.MakeItems();
            base.save(sender, e);
            Close();
        }
    }
}