using System.Collections.Generic;

namespace AndroidAppV2.Android_Product
{
    public class ProductCategory
    {
        private List<ProductSection> sections = new List<ProductSection>();

        public string Name { get; }

        public ProductCategory(string name) {
            Name = name;
        }

        public void AddSection(string section)
        {
            if (!sections.Exists(x => x.name == section))
                sections.Add(new ProductSection(section));
        }

        public void AddItemToSection(string section, string product)
        {
            if (sections.Exists(x => x.name == section))
            sections.Find(x => x.name == section).products.Add(new Product(product));
        }

        public List<Product> GetProductList(string section)
        {
            return sections.Find(x => x.name == section).products;
        } 
    }
}