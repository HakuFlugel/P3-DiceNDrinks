using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class ProductsController : ControllerBase
    {
        public List<Product> products = new List<Product>();
        public List<ProductCategory> categories = new List<ProductCategory>();

        public ProductsController(string path = "data/") : base(path)
        {
        }

        public event EventHandler ProductUpdated;

        public void addProduct(Product newProduct)
        {
            newProduct.id = new Random().Next();

            products.Add(newProduct);

            save();
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

            save();
            ProductUpdated?.Invoke(this, EventArgs.Empty);

        }

        public void removeProduct(Product oldProduct)
        {

            products.RemoveAll(p => p.id == oldProduct.id);
            //products.Remove(oldProduct);

            save();
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