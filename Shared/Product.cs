using System.Collections.Generic;

namespace Shared
{
    public class Product
    {
        public int id;
        public string name;
        public string image;
        public string category;
        public List<PriceElement> PriceElements = new List<PriceElement>();
        public Product(string name, string image) {
            this.name = name; this.image = image;
        }

        public Product() {

        }
    }
}