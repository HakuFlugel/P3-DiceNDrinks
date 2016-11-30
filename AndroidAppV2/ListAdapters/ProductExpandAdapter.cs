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
using Android;

namespace AndroidAppV2.ListAdapters {
    public class ExpandableDataAdapter : BaseExpandableListAdapter {

        readonly Activity Context;
        public ExpandableDataAdapter(Activity newContext, List<Product> newList, List<string> newGrpList) : base() {
            Context = newContext;
            ProductList = newList;
            GroupList = newGrpList;
        }

        protected List<Product> ProductList { get; set; }
        protected List<string> GroupList { get; set; }
        

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent) {
            View header = convertView;
            if (header == null) {
                header = Context.LayoutInflater.Inflate(Resource.Layout.productListGroup, null);
            }
            header.FindViewById<TextView>(Resource.Id.productSectionText).Text = GroupList[groupPosition];
            
            return header;
        }
        
        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent) {
            View row = convertView;
            if (row == null) {
                row = Context.LayoutInflater.Inflate(Resource.Layout.productListItem, null);
            }
            string newId = "", newValue = "";
            GetChildViewHelper(groupPosition, childPosition, out newId, out newValue);
            row.FindViewById<TextView>(Resource.Id.Text1).Text = newId;
            row.FindViewById<TextView>(Resource.Id.Text2).Text = newValue;

            return row;
            //throw new NotImplementedException ();
        }

        public override int GetChildrenCount(int groupPosition) {
            List<Product> results = ProductList.FindAll((Product obj) => obj.section == GroupList[groupPosition]);
            return results.Count;
        }

        public override int GroupCount {
            get {
                return GroupList.Count;
            }
        }

        private void GetChildViewHelper(int groupPosition, int childPosition, out string Name, out string Price) {
            List<Product> results = ProductList.FindAll((Product obj) => obj.section == GroupList[groupPosition]);
            Name = results[childPosition].name;
            Price = results[childPosition].PriceElements[0].price.ToString();
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

        public override bool HasStableIds {
            get {
                return true;
            }
        }

        #endregion
    }
}

