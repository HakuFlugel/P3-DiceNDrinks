using System.Collections.Generic;

namespace Shared
{
    public class ProductMenu
    {
        public List<ProductCategory> categories = new List<ProductCategory>(3);

        public ProductMenu()
        {
            categories[0].name = "Food";
            categories[1].name = "Drinks";
            categories[2].name = "Food";

        }
    }
}