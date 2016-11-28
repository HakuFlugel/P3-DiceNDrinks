using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Shared
{
    public class ProductsController : ControllerBase
    {
        List<Product> products = new List<Product>();
        List<ProductCategory> categories = new List<ProductCategory>(); //TODO: make stuff

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
            saveFile("data/products.json", products);
        }

        public override void load()
        {
            products = loadFile<Product>("data/products.json");
        }
        


    }
}