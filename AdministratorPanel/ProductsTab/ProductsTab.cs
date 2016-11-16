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

            flp.Controls.Add(addItemButton);
            flp.Controls.Add(tabControl);
           
            Controls.Add(flp);
        }

        public Product prmake(string name, string image, string cat, string section, List<PriceElement> pl) {
            Product pr = new Product();
            pr.name = name;
            pr.image = image;
            pr.category = cat;
            pr.section = section;
            pr.PriceElements = pl;

            return pr;
        }

        public void Generate() {
            PriceElement priceElementLittle = new PriceElement();

            priceElementLittle.name = "Little drink";
            priceElementLittle.price = 11;

            PriceElement priceElementBig = new PriceElement();
            priceElementBig.name = "Big drink";
            priceElementBig.price = 69;

            List<PriceElement> pricelist = new List<PriceElement>();
            pricelist.Add(priceElementLittle);
            pricelist.Add(priceElementBig);


            // food
            productList.Add(prmake("Asger","Red.png","Food","Meat", pricelist));
            productList.Add(prmake("shano", "_default.png", "Food", "Meat", pricelist));
            productList.Add(prmake("matias", "Red.png", "Food", "derp", pricelist));


            //drinks
            productList.Add(prmake("1", "Red.png", "drinks", "cola 1", pricelist));
            productList.Add(prmake("2", "Red.png", "drinks", "coffe 2", pricelist));
            productList.Add(prmake("3", "Red.png", "drinks", "stuff", pricelist));
            productList.Add(prmake("4", "Red.png", "drinks", "other", pricelist));

            // mics

            productList.Add(prmake("1", "Red.png", "mics", "1", pricelist));
            productList.Add(prmake("2", "Red.png", "mics", "2", pricelist));
            productList.Add(prmake("3", "Red.png", "mics", "3", pricelist));
            productList.Add(prmake("4", "Red.png", "mics", "4", pricelist));

        }
        
        public void Update() {
            MakeItems();
        }

        private void MakeItems() {
            tabControl.Controls.Clear();
            foreach (var item in productCategories) {
                ProductCategoryTab category = new ProductCategoryTab(item);
                tabControl.Controls.Add(category);
            }

            foreach (var item in productList) {
                //category
                //TODO: put this stuff in a function
                AddProductItem(new ProductItem(item, this));

            }
        }

        public void AddProductItem(ProductItem item) {
            var categoryResult = tabControl.Controls.Find(item.product.category, false);
            ProductCategoryTab categoryTab;

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
