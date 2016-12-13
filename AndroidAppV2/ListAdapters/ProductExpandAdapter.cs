using System;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using Shared;

namespace AndroidAppV2.ListAdapters {
    public class ExpandableDataAdapter : BaseExpandableListAdapter {

        readonly Activity _context;
        public ExpandableDataAdapter(Activity newContext, List<Product> newList, List<string> newGrpList) {
            _context = newContext;
            ProductList = newList;
            GroupList = newGrpList;
        }

        protected List<Product> ProductList { get; set; }
        protected List<string> GroupList { get; set; }


        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent) {
            View header = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.productListGroup, null);
            header.FindViewById<TextView>(Resource.Id.productSectionText).Text = GroupList[groupPosition];

            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent) {
            View row = convertView;
            AndroidShared androidshared = new AndroidShared();
            if (row == null) {
                row = _context.LayoutInflater.Inflate(Resource.Layout.productListItem, null);
            }
            string name, price, image;
            GetChildViewHelper(groupPosition, childPosition, out name, out price, out image);
            row.FindViewById<TextView>(Resource.Id.Text1).Text = name;
            row.FindViewById<TextView>(Resource.Id.Text2).Text = price;
            int[] sizes = { 75, 75 };
            row.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.nopic);
            androidshared.GetImages(image, row, Resource.Id.Image, sizes);

            return row;
        }

        public override int GetChildrenCount(int groupPosition) {
            List<Product> results = ProductList.FindAll(obj => obj.section == GroupList[groupPosition]);
            return results.Count;
        }

        public override int GroupCount => GroupList.Count;

        private void GetChildViewHelper(int groupPosition, int childPosition, out string name, out string price, out string image) {
            List<Product> results = ProductList.FindAll(obj => obj.section == GroupList[groupPosition]);
            name = results[childPosition].name;
            image = results[childPosition].image;
            switch (results[childPosition].PriceElements.Count) {
                case 1:
                    price = $"{results[childPosition].PriceElements[0].name} for {results[childPosition].PriceElements[0].price} kr.";
                    break;
                case 2:
                    price = $"{results[childPosition].PriceElements[0].name} for {results[childPosition].PriceElements[0].price} kr." + Environment.NewLine
                            + $"{results[childPosition].PriceElements[1].name} for {results[childPosition].PriceElements[1].price} kr.";
                    break;
                default:
                    price = $"Fra {results[childPosition].PriceElements[0].price} kr,-";
                    break;
            }
        }

        public Product GetTheProduct(int groupPosition, int childPosition) {
            List<Product> results = ProductList.FindAll(obj => obj.section == GroupList[groupPosition]);
            Product product = results[childPosition];
            return product;
        }

        #region implemented abstract members of BaseExpandableListAdapter

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition) {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition) {
            return childPosition;
        }

        public override Java.Lang.Object GetGroup(int groupPosition) {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition) {
            return groupPosition;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition) {
            return true;
        }

        public override bool HasStableIds => true;

        #endregion
    }
}

