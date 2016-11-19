using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidAppV2.ListAdapters;
using Shared;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Menu")]
    public class FoodmenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            
            
            SetContentView(Resource.Layout.foodmenuLayout);
            
            // Create your application here
            Button foodButton = FindViewById<Button>(Resource.Id.foodButton);
            ListView listView = FindViewById<ListView>(Resource.Id.list);
            List<Product> list = GenerateProductList();


            listView.ItemClick += OnListItemClick;

            foodButton.Click += delegate
            {
                foodButton.Enabled = true;
                List<Product> foodList = list.Where(prd => prd.category == "Food").ToList();
                ProductAdapter foodAdapter = new ProductAdapter(this, foodList);
                listView.Adapter = foodAdapter;
            };
            FindViewById<Button>(Resource.Id.drinkButton).Click += delegate
            {
                List<Product> drinksList = list.Where(prd => prd.category == "drinks").ToList();
                ProductAdapter drinksAdapter = new ProductAdapter(this, drinksList);
                listView.Adapter = drinksAdapter;
            };
            FindViewById<Button>(Resource.Id.miscButton).Click += delegate
            {
                List<Product> miscList = list.Where(prd => prd.category == "misc").ToList();
                ProductAdapter miscAdapter = new ProductAdapter(this, miscList);
                listView.Adapter = miscAdapter;
            };
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;

            //todo: dialog fragment med yderligere info om et item
        }

        private List<Product> GenerateProductList()
        {
            List<Product> productList = new List<Product>();
            PriceElement priceElementLittle = new PriceElement
            {
                name = "Little drink",
                price = 11
            };


            PriceElement priceElementBig = new PriceElement
            {
                name = "Big drink",
                price = 69
            };

            List<PriceElement> pricelist = new List<PriceElement> {priceElementLittle, priceElementBig};


            // food
            productList.Add(Prmake("Asger", "ProductTest/Red.png", "Food", "Meat", pricelist));
            productList.Add(Prmake("shano", "ProductTest/_default.png", "Food", "Meat", pricelist));
            productList.Add(Prmake("matias", "ProductTest/Red.png", "Food", "derp", pricelist));


            //drinks
            productList.Add(Prmake("1", "ProductTest/Red.png", "drinks", "cola 1", pricelist));
            productList.Add(Prmake("2", "ProductTest/Red.png", "drinks", "coffe 2", pricelist));
            productList.Add(Prmake("3", "ProductTest/Red.png", "drinks", "stuff", pricelist));
            productList.Add(Prmake("4", "ProductTest/Red.png", "drinks", "other", pricelist));


            // misc.
            productList.Add(Prmake("5", "ProductTest/Red.png", "misc", "1", pricelist));
            productList.Add(Prmake("6", "ProductTest/Red.png", "misc", "2", pricelist));
            productList.Add(Prmake("7", "ProductTest/Red.png", "misc", "3", pricelist));
            productList.Add(Prmake("8", "ProductTest/Red.png", "misc", "4", pricelist));

            return productList;
        }

        private static Product Prmake(string name, string image, string cat, string section, List<PriceElement> pl)
        {
            Product pr = new Product
            {
                name = name,
                image = image,
                category = cat,
                section = section,
                PriceElements = pl
            };

            return pr;
        }


    }
}