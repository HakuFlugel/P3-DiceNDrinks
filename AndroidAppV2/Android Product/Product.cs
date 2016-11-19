using System.Collections.Generic;

namespace AndroidAppV2.Android_Product
{
    public class Product
    {
        public int id;
        public string name;
        public string image;
        public List<PriceElement> PriceElements = new List<PriceElement>();
        public Product(string name) {
            this.name = name;
        }
    }
}