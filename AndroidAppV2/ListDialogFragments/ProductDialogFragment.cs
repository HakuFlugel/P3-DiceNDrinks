using System;

using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Shared;
using Java.Lang;

using AndroidAppV2.ListAdapters;
using Android.Content;

namespace AndroidAppV2.ListDialogFragments
{
    internal class ProductDialogFragment : DialogFragment
    {
        //private Button _buttonDismiss;
        private Product _product;
        private Context _context;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            //NO TITLE
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            //Create view
            View view = inflater.Inflate(Resource.Layout.ProductDialogView, container, true);

            StringBuilder sb = new StringBuilder();

            foreach (var item in _product.PriceElements) {
                sb.Append(item.name + " for " + item.price + " kr,-" + System.Environment.NewLine);
            }
            AndroidShared an = new AndroidShared();
            view.FindViewById<TextView>(Resource.Id.productName).Text = _product.name;
            int[] sizes = {150, 150}; //placeholder as we do not have larger images
            an.GetImages(_context, $"{_product.image}.png",view, Resource.Id.productImage,sizes);
            view.FindViewById<TextView>(Resource.Id.productPrices).Text = sb.ToString();
            
            //Button test dismiss
            //_buttonDismiss = view.FindViewById<Button>(Resource.Id.Button_Dismiss);
            //_buttonDismiss.Click += Button_Dismiss_Click;
            
            return view;
        }
        public void PassDataToFrag(Product product, Context context)
        {
            _product = product;
            _context = context;
        }

        public override void OnResume()
        {

            // Auto Size based on content
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            // No background behind the view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.DarkGray));

            base.OnResume();
        }
        /*private void Button_Dismiss_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _buttonDismiss.Click -= Button_Dismiss_Click;
            }
        }*/
    }
}