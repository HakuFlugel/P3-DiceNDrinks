﻿using System;
using NUnit.Framework;
using AdministratorPanel;
using Shared;
using System.Windows.Forms;
using System.Linq;

namespace NUnitTest {
    [TestFixture]
    public class ProductItemTest {
        Product product = new Product();
        FormProgressBar probar = new FormProgressBar();
        ProductsTab productTab;
        ProductItem productItem;

        public ProductItemTest() {
            productTab = new ProductsTab(probar);
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
            Label control = productItem.Controls.Find("ProductName", true).FirstOrDefault() as Label;

            Assert.True(control != null && control.Text == "Name");
            
        }

        [Test]
        public void PriceElementsInProductItem() {
            TableLayoutPanel control = productItem.Controls.Find("PriceTableName", true).FirstOrDefault() as TableLayoutPanel;

            Label[] priceElementsName = control.Controls.Find("PriceElementName", true) as Label[];
            Label[] priceElementsPrice = control.Controls.Find("PriceElementPrice", true) as Label[];

            for (int i = 0; i < product.PriceElements.Count(); i++) {

                Assert.True(product.PriceElements[i].name == priceElementsName[i].Text &&
                       product.PriceElements[i].price == Convert.ToDecimal(priceElementsPrice[i].Text));
            }
        }
    }
}








