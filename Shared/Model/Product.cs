﻿using System;
using System.Collections.Generic;

namespace Shared {
    public class Product {
        public int id;
        public DateTime timestamp;
        public string name;
        public string image;
        public string category;
        public string section;
        public List<PriceElement> PriceElements = new List<PriceElement>();
        public Product(string name, string image) {
            this.name = name; this.image = image;
        }

        public Product() {

        }

        public override string ToString() {
            return "name= " + name + "\n" +
                    "category= " + category + "\n" +
                    "section= " + section + "\n" +
                    "id= " + id + "\n";
        }
    }
}