using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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
            ListView listView = FindViewById<ListView>(Resource.Id.list);
            List<Product> list = GenerateProductList();

            ProductAdapter itemAdapter = new ProductAdapter(this, list);
            listView.Adapter = itemAdapter;
            listView.ItemClick += OnListItemClick;

            FindViewById<Button>(Resource.Id.foodButton).Click += delegate
            {
                List<Product> foodList = new List<Product>();
                foreach (Product prd in list)
                {
                    if (prd.category == "Food")
                    {
                        foodList.Add(prd);
                    }
                }
                ProductAdapter foodAdapter = new ProductAdapter(this, foodList);
                listView.Adapter = foodAdapter;
            };
            FindViewById<Button>(Resource.Id.drinkButton).Click += delegate
            {
                List<Product> drinksList = new List<Product>();
                foreach (Product prd in list)
                {
                    if (prd.category == "drinks")
                    {
                        drinksList.Add(prd);
                    }
                }
                ProductAdapter drinksAdapter = new ProductAdapter(this, drinksList);
                listView.Adapter = drinksAdapter;
            };
            FindViewById<Button>(Resource.Id.miscButton).Click += delegate
            {
                List<Product> miscList = new List<Product>();
                foreach (Product prd in list)
                {
                    if (prd.category == "misc")
                    {
                        miscList.Add(prd);
                    }
                }
                ProductAdapter miscAdapter = new ProductAdapter(this, miscList);
                listView.Adapter = miscAdapter;
            };
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;

            //todo: dialog fragment med yderligere info om et item
        }

        public List<Product> GenerateProductList()
        {
            List<Product> productList = new List<Product>();
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
            productList.Add(prmake("Asger", "Red.png", "Food", "Meat", pricelist));
            productList.Add(prmake("shano", "_default.png", "Food", "Meat", pricelist));
            productList.Add(prmake("matias", "Red.png", "Food", "derp", pricelist));


            //drinks
            productList.Add(prmake("1", "Red.png", "drinks", "cola 1", pricelist));
            productList.Add(prmake("2", "Red.png", "drinks", "coffe 2", pricelist));
            productList.Add(prmake("3", "Red.png", "drinks", "stuff", pricelist));
            productList.Add(prmake("4", "Red.png", "drinks", "other", pricelist));


            // misc.
            productList.Add(prmake("5", "Red.png", "misc", "1", pricelist));
            productList.Add(prmake("6", "Red.png", "misc", "2", pricelist));
            productList.Add(prmake("7", "Red.png", "misc", "3", pricelist));
            productList.Add(prmake("8", "Red.png", "misc", "4", pricelist));

            return productList;
        }

        public Product prmake(string name, string image, string cat, string section, List<PriceElement> pl)
        {
            Product pr = new Product();
            pr.name = name;
            pr.image = image;
            pr.category = cat;
            pr.section = section;
            pr.PriceElements = pl;

            return pr;
        }


    }
}