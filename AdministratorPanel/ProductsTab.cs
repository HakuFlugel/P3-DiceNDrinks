using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using System;

namespace AdministratorPanel {
    public class ProductsTab : AdminTabPage {
        List<ProductCategory> productCategories = new List<ProductCategory>();
        TabControl tabControl = new TabControl();

        public ProductsTab() {
            Text = "Products";

            tabControl.Dock = DockStyle.Fill;
            generateData();
            update();
            Controls.Add(tabControl);

        }

        public void update() {
            tabControl.Controls.Clear();
            foreach (var item in productCategories) {
                ProductCategoryTab category = new ProductCategoryTab(item);
                tabControl.Controls.Add(category);
            }

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
            priceElementLittle.price = 11;

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
            //throw new NotImplementedException();
        }

        public override void Load() {
            //throw new NotImplementedException();
        }
    }
}
