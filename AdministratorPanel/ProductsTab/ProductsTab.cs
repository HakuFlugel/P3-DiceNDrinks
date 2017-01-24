using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using Shared;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;

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

        public ProductsTab(FormProgressBar probar) {
            Text = "Products";
            tabControl.Dock = DockStyle.Fill;

            Load();
            download();


            MakeItems();
            probar.addToProbar();                               //For progress bar. 1

            addItemButton.Click += (s, e) => {
                new ProductPopupBox(this);
            };

            tableLayoutPanel.Controls.Add(addItemButton);
            probar.addToProbar();                               //For progress bar. 2
            tableLayoutPanel.Controls.Add(tabControl);
            probar.addToProbar();                               //For progress bar. 3
            Controls.Add(tableLayoutPanel);
            probar.addToProbar();                               //For progress bar. 4
        }

        public override void download() {
            try {
                string response = ServerConnection.sendRequest("/get.aspx",
                    new NameValueCollection() {
                        {"Type", "Products"}
                    }
                );

                Console.WriteLine(response);
                var tuple = JsonConvert.DeserializeObject<Tuple<List<ProductCategory>, List<Product>>>(response);

                Console.WriteLine(tuple.Item1);
                Console.WriteLine(tuple.Item2);

                foreach (var product in productList) {
                    if (!tuple.Item2.Any(g => g.image == product.image)) {
                        if (!string.IsNullOrEmpty(product.image) && File.Exists("images/products/" + product.image)) {
                            File.Delete("images/products/" + product.image);
                        }
                    }
                }

                foreach (var newproduct in tuple.Item2) {
                    try {
                        if (!String.IsNullOrEmpty(newproduct.image) && (!File.Exists("images/products/" + newproduct.image) || newproduct.timestamp > productList.FirstOrDefault(g => g.id == newproduct.id)?.timestamp)) {
                            if (File.Exists("images/products/" + newproduct.image)) {
                                File.Delete("images/products/" + newproduct.image);
                            }

                            using (WebClient client = new WebClient()) {
                                Console.WriteLine("http://" + ServerConnection.ip + "/images/products/" + newproduct.image);
                                client.DownloadFile(new Uri("http://" + ServerConnection.ip + "/images/products/" + newproduct.image), "images/products/" + newproduct.image);
                            }

                        }
                    } catch (WebException e) {
                        Console.WriteLine(e);
                    }

                }
                productCategories = tuple.Item1 ?? new List<ProductCategory>();
                productList = tuple.Item2 ?? new List<Product>();

                MakeItems();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
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
            Directory.CreateDirectory("Sources");
            if (!File.Exists(@"Sources/Products.json")) {
                File.Create(@"Sources/Products.json");
            }

            var jsonProducts = JsonConvert.SerializeObject(productList);
            using (StreamWriter textWriter = new StreamWriter(@"Sources/Products.json")) {
                foreach (var item in jsonProducts) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public override void Load() {
            string loadStringProducts = null;
            Directory.CreateDirectory("Sources");
            if (!File.Exists(@"Sources/Products.json")) {
                File.Create(@"Sources/Products.json");
            }

            using (StreamReader streamReader = new StreamReader(@"Sources/Products.json")) {
                loadStringProducts = streamReader.ReadToEnd();
                streamReader.Close();
            }

            if (loadStringProducts != null) {
                productList = JsonConvert.DeserializeObject<List<Product>>(loadStringProducts);
            }

            if (productList == null) {
                productList = new List<Product>();
            }
        }
    }
}
