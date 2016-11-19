using System.Collections.Generic;

namespace AndroidAppV2.Android_Product
{
    public class ProductSection
    {
        public string name;
        public List<Product> products = new List<Product>();

        public ProductSection(string name)
        {
            this.name = name;
        }
    }
}