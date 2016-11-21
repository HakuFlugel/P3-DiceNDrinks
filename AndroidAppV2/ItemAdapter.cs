using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Shared;

namespace AndroidAppV2
{
    class ItemAdapter : BaseAdapter<Product>
    {
        List<Product> items;
        Activity context;

        public ItemAdapter(Activity context, List<Product> items)
       : base()
   {
            this.context = context;
            this.items = items;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }

        public override int Count => items.Count;

        public override Product this[int position] => items[position];
    }
}