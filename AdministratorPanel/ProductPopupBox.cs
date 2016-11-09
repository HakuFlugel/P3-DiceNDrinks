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

namespace AdministratorPanel {
    class ProductPopupBox : FancyPopupBox {
        NiceTextBox productName = new NiceTextBox() {
            Width = 200,
            waterMark = "Product Name",
        };

        NiceTextBox categoryName = new NiceTextBox() {
            Width = 200,
            waterMark = "category Name",
        };

        Button priceElementBox = new Button() {
            Width = 80,
            Height = 20,
            AutoSize = true,
            Text = "Add Price",
            };

        Panel productImage = new Panel() {
            Width = 200,
            Height = 200,
            BackgroundImage = Image.FromFile("images/_default.png"),
            Dock = DockStyle.Top,
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        private ProductsTab productTab;
        private Product product;
        private DataTable dataTable = new DataTable();
        private Image image;
        private string imageName;

        public ProductPopupBox() {
        }

        public ProductPopupBox(ProductsTab productTab, Product product = null) {
            this.productTab = productTab;
            this.product = product;

            if (this.product != null) {
                productName.Text = this.product.name;
                categoryName.Text = this.product.category;

            } else {
                Controls.Find("delete", true).First().Enabled = false;
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
            DataGridView priceElements = new DataGridView();
            priceElements.Dock = DockStyle.Fill;
            priceElements.Height = 280;
            priceElements.RowHeadersVisible = false;
            priceElements.ScrollBars = ScrollBars.Vertical;
            priceElements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            priceElements.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            priceElements.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

          
            if (product != null) {
                // load priceElements 
                loadPriceElements(product.PriceElements);
                priceElements.DataSource = dataTable;
                //image
                try {
                    image = Image.FromFile(product.image);
                    productImage.BackgroundImage = image;

                } catch (Exception e) {
                    MessageBox.Show(e.Message);
                }
            } 
            
            productImage.Click += (s, e) => {
                //TODO: might be wr0ng

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
            throw new NotImplementedException();
        }

        protected override void save(object sender, EventArgs e) {

            if (product == null) {
                product = new Product();
                productTab.productList.Add(product);
            }// TODO: has category or section changed (else)
            
            product.name = productName.Text;
            product.PriceElements = SavePriceElemets();
            product.category = categoryName.Text;
            product.image = productImage.Name;
            if (image == null) {
                MessageBox.Show("No image in product " + product.name);
            } else {
                Console.WriteLine(imageName);
                image.Save("images/"+imageName);
            }

            Close();
        }
    }
}



// list of priceelements

//List<PriceElement> pr = new List<PriceElement>();
//pr.Add(new PriceElement() { name = "asger", price = 69 });
//pr.Add(new PriceElement() { name = "asger", price = 11 });
//pr.Add(new PriceElement() { name = "asger", price = 20 });
//pr.Add(new PriceElement() { name = "asger", price = 30 });
//pr.Add(new PriceElement() { name = "asger", price = 30 });
//pr.Add(new PriceElement() { name = "asger", price = 40 });
//pr.Add(new PriceElement() { name = "asger", price = 49 });
