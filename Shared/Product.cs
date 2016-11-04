﻿using System.Collections.Generic;

namespace Shared
{
    public class Product
    {
        public string name;
        public List<PriceElement> PriceElements = new List<PriceElement>();
        public Product(string name, string image) {
            this.name = name; this.image = image;
        }

        public Product() {

        }
        public string image;
    }
}