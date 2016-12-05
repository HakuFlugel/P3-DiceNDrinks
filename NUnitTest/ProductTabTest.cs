using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using Shared;
using AdministratorPanel;


namespace NUnitTest {
    [TestFixture]
    public class ProductTabTest {
        FormProgressBar probar = new FormProgressBar();
        ProductsTab productTab;
        List <ProductItem> productItems = new List<ProductItem>();

        public ProductTabTest() {
            productTab = new ProductsTab(probar);
            ProductItem pi = productItemMaker("Name1", "Red.png", "Food", "Burger");
            pi.Name = "TestOfProductItem";

            productItems.Add(pi);

            productItems.Add( productItemMaker("Name2", "Red.png", "Food", "Snacks"));
            productItems.Add( productItemMaker("Name3", "Red.png", "Drinks", "HotDrinks"));
            productItems.Add( productItemMaker("Name4", "Red.png", "Drinks", "HotDrinks"));
            productItems.Add( productItemMaker("Name5", "Red.png", "Mics", "Pass"));

        }

        public ProductItem productItemMaker(string name, string image, string cat, string section) {
            ProductItem pr = new ProductItem();
            Product p = new Product();

            p.name = name;
            p.image = image;
            p.category = cat;
            p.section = section;

            return new ProductItem(p,productTab);

        }

        [Test]
        public void AddNewProductItem() {

            foreach (var item in productItems) {
                productTab.AddProductItem(item);
            }

            ProductItem temp = productTab.Controls.Find("TestOfProductItem", true).FirstOrDefault() as ProductItem;

            Assert.True(temp != null && temp.product.name == "Name1" && temp.product.category == "Food");
        }

        [Test]
        public void CategoriesAndSectionsTest() {
            List<ProductCategory> productCatlist = new List<ProductCategory>();
            ProductCategory pr = new ProductCategory("Food");
            pr.sections.Add("Burger");
            pr.sections.Add("Snacks");
            productCatlist.Add(pr);

            pr = new ProductCategory("Drinks");
            pr.sections.Add("HotDrinks");
            pr.sections.Add("HotDrinks");
            productCatlist.Add(pr);

            pr = new ProductCategory("Mics");
            pr.sections.Add("Pass");
            productCatlist.Add(pr);
            

            for (int i = 0; i < productTab.productCategories.Count; i++) {
               Assert.True(productTab.productCategories[i].name == productCatlist[i].name);

                for (int j = 0; j < productTab.productCategories[i].sections.Count; j++) {
                    Assert.True(productTab.productCategories[i].sections[j] == productCatlist[i].sections[j]);
                }
            }

        }

    }
}
