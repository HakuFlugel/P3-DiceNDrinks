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
using Java.Lang;

namespace AndroidAppV2.ListAdapters {
    class ProductExpandAdapter : BaseExpandableListAdapter {





        public override int GroupCount {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool HasStableIds {
            get {
                throw new NotImplementedException();
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition) {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition) {
            throw new NotImplementedException();
        }

        public override int GetChildrenCount(int groupPosition) {
            throw new NotImplementedException();
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent) {
            throw new NotImplementedException();
        }

        public override Java.Lang.Object GetGroup(int groupPosition) {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition) {
            throw new NotImplementedException();
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent) {
            throw new NotImplementedException();
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition) {
            throw new NotImplementedException();
        }
    }
}