using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace AdministratorPanel {
    public class ProductsTab : AdminTabPage {
        public List<ProductCategory> productCategories = new List<ProductCategory>();

        public List<Product> productList = new List<Product>();

        private TabControl tabControl = new TabControl();

        public ProductsTab() {
            //name for the tab
            Text = "Products";

            tabControl.Dock = DockStyle.Fill;
            Load();
            MakeItems();

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.ColumnCount = 1;

            Button addItemButton = new Button();
            addItemButton.Height = 20;
            addItemButton.Width = 100;
            addItemButton.Dock = DockStyle.Right;
            addItemButton.Text = "Add product";
            addItemButton.Click += (s, e) => {
                ProductPopupBox p = new ProductPopupBox(this);
                p.Show();
            };

            tableLayoutPanel.Controls.Add(addItemButton);
            tableLayoutPanel.Controls.Add(tabControl);
           
            Controls.Add(tableLayoutPanel);
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
            var jsonCategory = JsonConvert.SerializeObject(productCategories);
            Directory.CreateDirectory("Sources");
            using (StreamWriter textWriter = new StreamWriter(@"Sources/Category.json")) {
                foreach (var item in jsonCategory) {
                    textWriter.Write(item.ToString());
                }
            }

            var jsonProducts = JsonConvert.SerializeObject(productList);
            using (StreamWriter textWriter = new StreamWriter(@"Sources/Products.json")) {
                foreach (var item in jsonProducts) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public override void Load() {
            string loadStringCategory;
            if (File.Exists(@"Sources/Category.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/Category.json")) {
                    loadStringCategory = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                
                if (loadStringCategory != null) {
                    productCategories = JsonConvert.DeserializeObject<List<ProductCategory>>(loadStringCategory);
                }
            }

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
