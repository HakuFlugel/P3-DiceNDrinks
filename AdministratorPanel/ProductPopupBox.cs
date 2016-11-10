using System;
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

        Button priceElementBox = new Button() {
            Width = 80,
            Height = 20,
            AutoSize = true,
            Text = "Add Price",
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
        private Product product;
        private DataTable dataTable = new DataTable();
        private Image image;
        private string imageName = "_default.png";

        public ProductPopupBox() { }

        public ProductPopupBox(ProductsTab productTab, Product product = null) {
            this.productTab = productTab;
            this.product = product;


            if (this.product != null) {
                productName.Text = this.product.name;
                categoryName.Text = this.product.category;
                sectionName.Text = this.product.section;
                imageName = this.product.image;
                Console.WriteLine(imageName + " p -> " + product.image);
            } else {
                Controls.Find("delete", true).First().Enabled = false;
            }
            update();
        }

        //try {
        //    image = Image.FromFile("images/" + imageName);
        //} catch (Exception) {
        //    image = Image.FromFile("images/_default.png");
        //    MessageBox.Show("no image in product");
        //}

        private void update() {
            categoryName.Items.Clear();
            // update dropdown category
            foreach (ProductCategory item in productTab.productCategories) {
                categoryName.Items.Add(item.name);
            }
            // update dropdown section
            if (categoryName.Text != "Category Name" || categoryName.Text != "") {
                ProductCategory cat = productTab.productCategories.First(o => o.name == categoryName.Text);
                sectionName.Items.AddRange(cat.sections.ToArray());
            }

            if (product.PriceElements != null) {
                loadPriceElements(product.PriceElements);
                priceElements.DataSource = dataTable;
            }

            try {
                image = Image.FromFile("images/" + product.image);
                productImage.BackgroundImage = image;

            } catch (Exception e) {
                MessageBox.Show(e.Message);
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

            // priceElements list display                   // se om den kan komme i kroppen af priceElemets
            priceElements.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            // Add button click event
            productImage.Click += (s, e) => {
                //TODO: might be wr0ng

                // open file dialog for image selection
                OpenFileDialog ofd = new OpenFileDialog();
                var pnis = ImageCodecInfo.GetImageDecoders();
                StringBuilder sb = new StringBuilder();

                foreach (ImageCodecInfo item in pnis) {
                    sb.Append(item.FilenameExtension);
                    sb.Append(";");
                }

                ofd.Title = "Open Image";
                ofd.Filter = "Image Files | " + sb.ToString() ;

                if (ofd.ShowDialog() == DialogResult.OK) {
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
            DataTable dt = new DataTable();
            dt.Columns.Add("Name",typeof(string));
            dt.Columns.Add("Price", typeof(decimal));

            foreach (var item in priceElements) {

                dt.Rows.Add(item.name, item.price);
            }
            dataTable = dt;
        }
        
        private List<PriceElement> SavePriceElemets() {
            List<PriceElement> pr = new List<PriceElement>();

            int index = dataTable.Rows.Count;
            PriceElement priceElement = new PriceElement();
            

            for (int first = 0; first < index; first++) { 
                priceElement.name = dataTable.Rows[first][0].ToString(); // string(name)
                priceElement.price = decimal.Parse(dataTable.Rows[first][1].ToString()); // decimal(price)
                pr.Add(priceElement);
            }

            return pr;
        }

        protected override void delete(object sender, EventArgs e) {
            productTab.productList.Remove(product);
        }

        protected override void save(object sender, EventArgs e) {

            if (product == null) {
                product = new Product();
                productTab.productList.Add(product);
            }
            
            product.name = productName.Text;
            product.PriceElements = SavePriceElemets();
            product.category = categoryName.Text;
            product.image = imageName;
            Console.WriteLine("Save: imagename =" + imageName + " product image =" + product.image);
            if (image == null) {
                MessageBox.Show("No image in product " + product.name);
            } else {
                Console.WriteLine(imageName);
                if (!File.Exists("images/" + imageName)) {
                    image.Save("images/" + imageName);
                }
            }
            Close();
        }
    }
}