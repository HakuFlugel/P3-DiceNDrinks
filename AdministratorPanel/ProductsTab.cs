using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace AdministratorPanel {
    public class ProductsTab : AdminTabPage {
        public List<ProductCategory> productCategories = new List<ProductCategory>();

        public List<Product> productList = new List<Product>();

        private TabControl tabControl = new TabControl();

        public ProductsTab() {
            Text = "Products";

            tabControl.Dock = DockStyle.Fill;
            //Load();
            Generate();
            MakeItems();


            TableLayoutPanel flp = new TableLayoutPanel();
            flp.Dock = DockStyle.Fill;
            flp.AutoSize = true;
            flp.RowCount = 2;
            flp.ColumnCount = 1;

            Button addItemButton = new Button();
            addItemButton.Height = 20;
            addItemButton.Width = 100;
            addItemButton.Dock = DockStyle.Right;
            addItemButton.Text = "Add product";
            addItemButton.Click += (s, e) => {
                ProductPopupBox p = new ProductPopupBox(this);
                p.Show();
            };

            foreach (var item in productCategories) {
                Console.WriteLine(item.name);
            }

            flp.Controls.Add(addItemButton);
            flp.Controls.Add(tabControl);
           
            Controls.Add(flp);
        }


        public void Generate() {
            Product product = new Product("Name of product", "Red.png");
            PriceElement priceElementLittle = new PriceElement();
            product.category = "Food";
            product.section = "Meat";
            priceElementLittle.name = "Little drink";
            priceElementLittle.price = 11;

            PriceElement priceElementBig = new PriceElement();
            priceElementBig.name = "Big drink";
            priceElementBig.price = 69;

            product.PriceElements.Add(priceElementLittle);
            product.PriceElements.Add(priceElementBig);

            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
            productList.Add(product);
        }
        
        public void Update() {
            MakeItems();
        }

        private void MakeItems() {
            Controls.Clear();
            foreach (var item in productCategories) {
                ProductCategoryTab category = new ProductCategoryTab(item);
                tabControl.Controls.Add(category);
            }

            foreach (var item in productList) {
                var categoryResult = tabControl.Controls.Find(item.category, false);
                ProductCategoryTab categoryTab;
                if (categoryResult.Count() == 0) {
                    ProductCategory category = new ProductCategory(item.category);
                    categoryTab = new ProductCategoryTab(new ProductCategory(item.category));
                    tabControl.Controls.Add(categoryTab);
                    productCategories.Add(category);
                } else {
                    categoryTab = categoryResult.First() as ProductCategoryTab;

                }

                var sectionResult = categoryTab.Controls.Find(item.section, false);
                ProductSectionItem section;
                if (sectionResult.Count() == 0) {
                    section = new ProductSectionItem(item.section);
                    categoryTab.Controls.Add(section);
                } else {
                    section = sectionResult.First() as ProductSectionItem;
                }
                section.AddItem(new ProductItem(item, this));
            }

            foreach (var item in productCategories) {
                Console.WriteLine(item.name);
            }
        }

        private List<Product> productlist() {
            List<Product> productList = new List<Product>();
            Product tempProduct = new Product("Name of product", "Red.png");

            PriceElement priceElementLittle = new PriceElement();
            priceElementLittle.name = "Little drink";
            priceElementLittle.price = 68;

            PriceElement priceElementBig = new PriceElement();
            priceElementBig.name = "Big drink";
            priceElementBig.price = 69;


            tempProduct.PriceElements.Add(priceElementLittle);
            tempProduct.PriceElements.Add(priceElementBig);

            for (int i = 0; i < 10; i++) {
                productList.Add(tempProduct);
            }

            return productList;
        }

        public override void Save() {
            //var json = JsonConvert.SerializeObject(productCategories);
            //if (File.Exists(@"Sources/Category.json"))
            //    File.Create(@"Sources/Category.json");
            //using (StreamWriter textWriter = new StreamWriter(@"Sources/Category.json")) {
            //    foreach (var item in json) {
            //        textWriter.Write(item.ToString());
            //    }
            //}
        }

        public override void Load() {
            //string input;
            //if (File.Exists(@"Sources/Category.json")) {
            //    using (StreamReader streamReader = new StreamReader(@"Sources/Category.json")) {
            //        input = streamReader.ReadToEnd();
            //        streamReader.Close();
            //    }
                
            //    if (input != null) {
            //        productCategories = JsonConvert.DeserializeObject<List<ProductCategory>>(input);
            //    }


            //}

        }
    }
}
