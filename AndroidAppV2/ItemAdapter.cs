using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Provider;
using Android.Widget;
using Object = Java.Lang.Object;

namespace AndroidAppV2
{
    class ItemAdapter : BaseAdapter
    {

        //TODO: Ændrer fra Contact til Item
        //https://developer.xamarin.com/recipes/android/data/adapters/create_a_custom_adapter_for_contacts/

        List<Contact> _contactList;
        Activity _activity;

        public ItemAdapter(Activity activity)
        {
            _activity = activity;
            FillContacts();
        }

        void FillContacts()
        {
            var uri = ContactsContract.Contacts.ContentUri;

            string[] projection = {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                ContactsContract.Contacts.InterfaceConsts.PhotoId
            };
            //TODO: ændre til items og ikke contact

            var cursor = _activity.ManagedQuery(uri, projection, null,
                null, null);

            _contactList = new List<Contact>();

            if (cursor.MoveToFirst())
            {
                do
                {
                    _contactList.Add(new Contact
                    {
                        Id = cursor.GetLong(
                    cursor.GetColumnIndex(projection[0])),
                        DisplayName = cursor.GetString(
                    cursor.GetColumnIndex(projection[1])),
                        PhotoId = cursor.GetString(
                    cursor.GetColumnIndex(projection[2]))
                    });
                } while (cursor.MoveToNext());
            }
        }

        class Contact
        {
            public long Id { get; set; }
            public string DisplayName { get; set; }
            public string PhotoId { get; set; }
        }

        public override Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return _contactList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(
        Resource.Layout.foodmenuListItem, parent, false);
            var contactName = view.FindViewById<TextView>(Resource.Id.ItemName);
            var contactImage = view.FindViewById<ImageView>(Resource.Id.ItemImage);
            contactName.Text = _contactList[position].DisplayName;

            if (_contactList[position].PhotoId == null)
            {
                contactImage = view.FindViewById<ImageView>(Resource.Id.ItemImage);
                contactImage.SetImageResource(Resource.Drawable.Icon); //TODO: Skal ændres til billede hentet fra serveren
            }
            else
            {
                var contactUri = ContentUris.WithAppendedId(
                    ContactsContract.Contacts.ContentUri, _contactList[position].Id);
                var contactPhotoUri = Android.Net.Uri.WithAppendedPath(contactUri,
                    Contacts.Photos.ContentDirectory);
                contactImage.SetImageURI(contactPhotoUri);
            }
            return view;

        }

        public override int Count => _contactList.Count;
    }
}