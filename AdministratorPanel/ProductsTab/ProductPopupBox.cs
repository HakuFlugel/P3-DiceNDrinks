using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Shared;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;

namespace AdministratorPanel {
     public class ProductPopupBox : FancyPopupBox {
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

        public ProductPopupBox(ProductsTab productTab, ProductItem productItem = null) {

            Focus();
            Text = "Product";

            this.productTab = productTab;
            this.productItem = productItem;


            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));

            if (this.productItem != null) {
                productName.Text = this.productItem.product.name;
                categoryName.Text = this.productItem.product.category;
                sectionName.Text = this.productItem.product.section;
                imageName = this.productItem.product.image;

                loadPriceElements(productItem.product.PriceElements);

                if (productItem.product.image != null) {
                    try {
                        image = Image.FromFile("images/" + productItem.product.image);
                        productImage.BackgroundImage = image;

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
            categoryName.SelectedValueChanged += (s, e) => {
                sectionName.Items.Clear();
                if (categoryName.Text != "Category Name" && categoryName.Text != "") {
                    ProductCategory cat = productTab.productCategories.First(o => o.name == categoryName.Text);
                    sectionName.Items.AddRange(cat.sections.ToArray());
                    sectionName.Text = "Section Name";
                }
                hasBeenChanged = (productItem != null) ? productItem.product.category != categoryName.Text ? true : false : true;
            };
            priceElements.CellValueChanged += (s, e) => { hasBeenChanged = true; };
            productName.TextChanged += (s, e) => { hasBeenChanged = (productItem != null) ? productItem.product.name != productName.Text ? true : false : true; };
            sectionName.SelectedValueChanged += (s, e) => { hasBeenChanged = (productItem != null) ? productItem.product.section != sectionName.Text ? true : false : true; };
            //productImage.BackgroundImageChanged += (s, e) => {hasBeenChanged = (productItem != null) ? Image.FromFile("images/" + productItem.product.image) != productItem.BackgroundImage };


        }
        
        private void update() {
            categoryName.Items.Clear();
            // update dropdown category
            foreach (ProductCategory item in productTab.productCategories) {
                categoryName.Items.Add(item.name);
            }

            
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
                StringBuilder sB = new StringBuilder();

                foreach (ImageCodecInfo item in imageCodeInfo) {
                    sB.Append(item.FilenameExtension);
                    sB.Append(";");
                }

                ofd.Title = "Open Image";
                ofd.Filter = "Image Files | " + sB.ToString() ;

                if (ofd.ShowDialog() == DialogResult.OK) {

                    if (File.Exists("images/" + ofd.SafeFileName)) {
                        NiceMessageBox.Show("imagename already exist");
                    }

                    try {
                        imageName =  ofd.SafeFileName; // name
                        image = Image.FromFile( ofd.FileName); // path + name
                        productImage.BackgroundImage = image; 
                        
                    } catch (Exception ex) {
                        NiceMessageBox.Show(ex.Message);
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

                    dataTable.Rows.Add(item.name, item.price.ToString());
                }
            }
        }
        
        private List<PriceElement> SavePriceElemets() {
            List<PriceElement> pr = new List<PriceElement>();

            int index = dataTable.Rows.Count;

            for (int row = 0; row < index; row++) {
                bool justReturn = false;
                decimal bob = 0;
                try {
                    bob = decimal.Parse(dataTable.Rows[row][1].ToString());
                } catch (Exception) {
                    justReturn = true;
                }

                if ((dataTable.Rows[row][0] != DBNull.Value && dataTable.Rows[row][1] == DBNull.Value)||justReturn) {
                    NiceMessageBox.Show($"Invalid price on row {row+1}.\nThe product was not saved");
                    return null;
                } else if (dataTable.Rows[row][0] == DBNull.Value && dataTable.Rows[row][1] == DBNull.Value) {
                    continue;
                }
                
                PriceElement priceElement = new PriceElement();
                priceElement.name = dataTable.Rows[row][0].ToString(); // string(name)

                priceElement.price = bob; // decimal(price)
                pr.Add(priceElement);
            }
            return pr;
        }

        protected override void delete(object sender, EventArgs e) {
            productTab.productList.Remove(productItem.product);
        }

        protected override void save(object sender, EventArgs e) {
            // Image save
            Directory.CreateDirectory("images");

            if (image == null) {
                if (DialogResult.OK != NiceMessageBox.Show("No image in product. Do you still want to save", "No Images", MessageBoxButtons.OKCancel)) {
                    return;
                }

                image = productImage.BackgroundImage;
            } else {
                Console.WriteLine(imageName);
                if (!File.Exists("images/" + imageName)) {
                    image.Save("images/" + imageName);
                }
            }
            // convert priceelements to saves
            List<PriceElement> listOfPriceElements = SavePriceElemets();
            if (listOfPriceElements == null) {
                return;
            }
            //make new productSave
            if (productItem == null) {
                Product product = new Product();

                product.name = productName.Text;
                product.PriceElements = listOfPriceElements;
                product.category = categoryName.Text;
                product.section = sectionName.Text;
                product.image = imageName;

                productItem = new ProductItem(product, productTab);

                productTab.productList.Add(productItem.product);
                productTab.AddProductItem(productItem);

                Close();
                return;

            } // override a product
            else if (categoryName.Text != productItem.product.category || sectionName.Text != productItem.product.section) {
                productItem.Parent.Controls.Remove(productItem);
                productTab.AddProductItem(productItem);
            } 
            // save content
            productItem.product.name = productName.Text;
            productItem.product.PriceElements = listOfPriceElements;
            productItem.product.category = categoryName.Text;
            productItem.product.section = sectionName.Text;
            productItem.product.image = imageName;

            // updates producttab elements
            
            productTab.MakeItems();

            Close();
        }
    }
}