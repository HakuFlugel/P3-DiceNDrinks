using System;
using System.Collections.Generic;
using System.IO;
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

        public event EventHandler<UpdateGameEventArgs> GameUpdated;
        public class UpdateGameEventArgs
        {
            public Product newProduct;
            public Product oldProduct;

            public UpdateGameEventArgs(Product oldProduct, Product newProduct)
            {
                this.oldProduct = oldProduct;
                this.newProduct = newProduct;
            }
        }
//TODO: cut+pasta category+section stuff from producttab(popup)
        public void addGame(Product newProduct)
        {
            newProduct.id = new Random().Next(); // TODO: unique id

            products.Add(newProduct);

            GameUpdated?.Invoke(this, new UpdateGameEventArgs(null, newProduct));
        }


        public void updateGame(Product oldProduct, Product newProduct)
        {
            //newProduct.created = oldProduct.created;
            newProduct.id = oldProduct.id; // TODO: needed?

            products[products.FindIndex(e => e == oldProduct)] = newProduct;

            GameUpdated?.Invoke(this, new UpdateGameEventArgs(oldProduct, newProduct));

        }

        public void removeGame(Product oldProduct)
        {

            products.Remove(oldProduct); //TODO: make sure this uses id to compare

            GameUpdated?.Invoke(this, new UpdateGameEventArgs(oldProduct, null));
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