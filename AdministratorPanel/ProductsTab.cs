using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AdministratorPanel {
    public class ProductsTab : AdminTabPage {
        List<ProductCategory> productCategories = new List<ProductCategory>();// opdel til at kun have producter i en liste

        public List<Product> productList = new List<Product>();
        private TabControl tabControl = new TabControl();
        public List<string> categoryNames = new List<string>();

        public ProductsTab() {
            Text = "Products";

            tabControl.Dock = DockStyle.Fill;
            generateData();
            update();

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

            flp.Controls.Add(addItemButton);
            flp.Controls.Add(tabControl);
           
            Controls.Add(flp);
        }

        public void update() {
            tabControl.Controls.Clear();
            foreach (var item in productCategories) {
                ProductCategoryTab category = new ProductCategoryTab(item);
                tabControl.Controls.Add(category);
            }
            Load();
        }



        public void generateData() {

            for (int i = 0; i < 3; i++) {
                ProductCategory category = new ProductCategory();
                category.name = "i = " + i;
                productCategories.Add(category);

                for (int j = 0; j < 5; j++) {
                    ProductSection section = new ProductSection();
                    section.name = "j = " + j;
                    category.sections.Add(section);
                    for (int k = 0; k < 15; k++) {

                        Product product = new Product("Name of product", "images/Red.png");
                        PriceElement priceElementLittle = new PriceElement();

                        priceElementLittle.name = "Little drink";
                        priceElementLittle.price = 11;

                        PriceElement priceElementBig = new PriceElement();
                        priceElementBig.name = "Big drink";
                        priceElementBig.price = 69;

                        product.PriceElements.Add(priceElementLittle);
                        product.PriceElements.Add(priceElementBig);

                        section.products.Add(product);
                    }
                }
            }
        }


        private List<Product> productlist() {
            List<Product> productList = new List<Product>();
            Product tempProduct = new Product("Name of product", "images/Red.png");

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
            //var json = JsonConvert.SerializeObject(categoryNames);
            //using (StreamWriter textWriter = new StreamWriter(@"Scourses/Category.json")) {
            //    foreach (var item in json) {
            //        textWriter.Write(item.ToString());
            //    }
            //}
        }

        public override void Load() {
            //string input;
            //using (StreamReader streamReader = new StreamReader(@"Scourses/Category.json")) {
            //    input = streamReader.ReadToEnd();
            //}
            //if (input != null) {
            //    categoryNames = JsonConvert.DeserializeObject<List<string>>(input);
            //}
        }
    }
}
