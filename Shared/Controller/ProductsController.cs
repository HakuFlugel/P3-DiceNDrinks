using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft;

namespace Shared
{
    public class ProductsController : ControllerBase
    {
        public List<Product> products = new List<Product>();
        public List<ProductCategory> categories = new List<ProductCategory>(); //TODO: make stuff

        public ProductsController(string path = "data/") : base(path)
        {
        }

        public event EventHandler ProductUpdated;

//TODO: cut+pasta category+section stuff from producttab(popup)
        public void addProduct(Product newProduct)
        {
            newProduct.id = new Random().Next(); // TODO: unique id

            products.Add(newProduct);

            ProductUpdated?.Invoke(this, EventArgs.Empty);
        }


        public void updateProduct(Product newProduct)
        {

            Product oldProduct = products.First(p => p.id == newProduct.id);

            if (oldProduct == null)
            {
                return;
            }

            products[products.FindIndex(e => e == oldProduct)] = newProduct;

            ProductUpdated?.Invoke(this, EventArgs.Empty);

        }

        public void removeProduct(Product oldProduct)
        {

            products.Remove(oldProduct); //TODO: make sure this uses id to compare

            ProductUpdated?.Invoke(this, EventArgs.Empty);
        }

        public override void save()
        {
            saveFile("products", products);
        }

        public override void load()
        {
            products = loadFile<Product>("products");
        }



    }
}