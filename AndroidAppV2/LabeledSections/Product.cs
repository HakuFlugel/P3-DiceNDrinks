using System;
using System.Collections.Generic;
using Java.Util.Jar;
using Shared;

namespace AndroidAppV2.LabeledSections
{
	class Product : Java.Lang.Object, IHasLabel, IComparable<Product>
	{
        public int id;
        public string name;
        public string image;
        public string category;
        public string section;
        public List<PriceElement> PriceElements = new List<PriceElement>();

        public Product(string name, string image, string section)
        {
            this.name = name; this.image = image;
        }

        public Product()
        {

        }

        public string Label => section;


        int IComparable<Product>.CompareTo (Product value)
		{
			return id.CompareTo (value.id);
		}
		

	}
}



