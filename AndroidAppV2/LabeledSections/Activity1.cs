using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Shared;

namespace AndroidAppV2.LabeledSections
{
	[Activity (Label = "Labelled Sections")]
	public class ListWithHeaders: ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			var data = new ListItemCollection<Product> () {
				new Product("derp","faggot") 
			};
			
			var sortedContacts = data.GetSortedData ();
			var adapter = CreateAdapter (sortedContacts);
			ListAdapter = adapter;

		}
		
		SeparatedListAdapter CreateAdapter<T> (Dictionary<string, List<T>> sortedObjects)
			where T : IHasLabel, IComparable<T>
		{
			var adapter = new SeparatedListAdapter (this);
			foreach (var e in sortedObjects.OrderBy (de => de.Key)) {
				var label   = e.Key;
				var section = e.Value;
				adapter.AddSection (label, new ArrayAdapter<T> (this, Resource.Layout.ListItem, section));
			}
			return adapter;
		}
	}
}


