using System;
using NUnit.Framework;
using AdministratorPanel;
using Shared;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace NUnitTest {
    [TestFixture]
    public class ProductItemTest {
        Product product = new Product();
        ProductsTab productTab = new ProductsTab();
        ProductItem productItem;

        public ProductItemTest() {
            product.name = "Name";
            product.image = "Red.png";
            product.category = "Food";
            product.section = "Burger";

            PriceElement pr = new PriceElement();
            pr.name = "One";
            pr.price = 15;

            productItem = new ProductItem(product, productTab);
        }

        [Test]
        public void CategoryandSectionSetTest() {
            
            Assert.True(productItem.product.section == "Burger" 
                      && productItem.product.category == "Food");
        }

        [Test]
        public void ProductNameInProductItem() {
            Label ctr = productItem.Controls.Find("ProductName", true).FirstOrDefault() as Label;

            Assert.True(ctr != null && ctr.Text == "Name");
            
        }

        [Test]
        public void PriceElementsInProductItem() {
            TableLayoutPanel ctr = productItem.Controls.Find("PriceTableName", true).FirstOrDefault() as TableLayoutPanel;

            Label[] priceElementsName = ctr.Controls.Find("PriceElementName", true) as Label[];
            Label[] priceElementsPrice = ctr.Controls.Find("PriceElementPrice", true) as Label[];

            for (int i = 0; i < product.PriceElements.Count(); i++) {

                Assert.True(product.PriceElements[i].name == priceElementsName[i].Text &&
                       product.PriceElements[i].price == Convert.ToDecimal(priceElementsPrice[i].Text));
            }
        }
    }
}








