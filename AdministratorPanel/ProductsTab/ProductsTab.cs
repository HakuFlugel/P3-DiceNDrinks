using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace AdministratorPanel {
    public class ProductsTab : AdminTabPage {

        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            AutoSize = true,
            RowCount = 2,
            ColumnCount = 1
        };

        private Button addItemButton = new Button() {
            Height = 20,
            Width = 100,
            Dock = DockStyle.Right,
            Text = "Add product"
        };

        public List<ProductCategory> productCategories = new List<ProductCategory>();
        public List<Product> productList = new List<Product>();
        public TabControl tabControl = new TabControl();
        private FormProgressBar probar;

        public ProductsTab(FormProgressBar probar) {
            Text = "Products";
            this.probar = probar;
            tabControl.Dock = DockStyle.Fill;

            Load();
            MakeItems();
            probar.addToProbar();                               //For progress bar. 1

            addItemButton.Click += (s, e) => {
                ProductPopupBox p = new ProductPopupBox(this);
            };
            
            tableLayoutPanel.Controls.Add(addItemButton);
            probar.addToProbar();                               //For progress bar. 2
            tableLayoutPanel.Controls.Add(tabControl);
            probar.addToProbar();                               //For progress bar. 3
            Controls.Add(tableLayoutPanel);
            probar.addToProbar();                               //For progress bar. 4
        }

        public void MakeItems() {
            tabControl.Controls.Clear();

            foreach (var item in productCategories) {
                ProductCategoryTab category = new ProductCategoryTab(item);
                tabControl.Controls.Add(category);
            }

            foreach (var item in productList) {
                AddProductItem(new ProductItem(item, this));
            }
        }

        public void AddProductItem(ProductItem item) {
            var categoryResult = tabControl.Controls.Find(item.product.category, false);
            ProductCategoryTab categoryTab;

            //category
            if (categoryResult.Count() == 0) {
                ProductCategory category = new ProductCategory(item.product.category);
                categoryTab = new ProductCategoryTab(category);
                tabControl.Controls.Add(categoryTab);
                productCategories.Add(category);
            } else {
                categoryTab = categoryResult.First() as ProductCategoryTab;
            }

            //section
            var sectionResult = categoryTab.table.Controls.Find(item.product.section, false);
            ProductSectionItem section;

            if (sectionResult.Count() == 0) {
                section = new ProductSectionItem(item.product.section);
                categoryTab.category.sections.Add(section.Name);
                categoryTab.table.Controls.Add(section);

            } else {
                section = sectionResult.First() as ProductSectionItem;
            }

            section.AddItem(item);
        }

        public override void Save() {
            var jsonProducts = JsonConvert.SerializeObject(productList);
            using (StreamWriter textWriter = new StreamWriter(@"Sources/Products.json")) {
                foreach (var item in jsonProducts) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public override void Load() {
            string loadStringProducts;

            if (File.Exists(@"Sources/Products.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/Products.json")) {
                    loadStringProducts = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (loadStringProducts != null) {
                    productList = JsonConvert.DeserializeObject<List<Product>>(loadStringProducts);
                }
            }

            if (productList == null) {
                productList = new List<Product>();
            }
        }
    }
}
