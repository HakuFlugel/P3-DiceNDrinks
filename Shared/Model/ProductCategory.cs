using System.Collections.Generic;

namespace Shared {
    public class ProductCategory {
        public string name;

        public List<string> sections = new List<string>();

        public ProductCategory(string name) {
            this.name = name;
        }
    }
}