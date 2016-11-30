using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using AdministratorPanel;

namespace NUnitTest {
    [TestFixture]
    public class ProductTabTest {

        ProductsTab productTab = new ProductsTab();
        ProductItem productItem;


        public ProductTabTest() {
            Product product = new Product(); ;

            product.name = "Name";
            product.image = "Red.png";
            product.category = "Food";
            product.section = "Burger";

            PriceElement pr = new PriceElement();
            pr.name = "One";
            pr.price = 15;

            productItem = new ProductItem(product, productTab);
            productItem.Name = "TestOfProductItem";
        }


        [Test]
        public void AddNewProductItem() {

            productTab.AddProductItem(productItem);

            ProductItem temp = productTab.Controls.Find("TestOfProductItem", true).FirstOrDefault() as ProductItem;

            Assert.True(temp != null && temp.product.name == "Name" && temp.product.category == "Food");

        }

        [Test]
        public void New() {

           

        }
    }
}
