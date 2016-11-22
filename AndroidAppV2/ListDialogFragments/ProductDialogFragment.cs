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

namespace AndroidAppV2.ListDialogFragments
{
    internal class ProductDialogFragment : DialogFragment
    {
        private Button _buttonDismiss;
        private TextView _textSomething;
        private Product _product;
        private ProductAdapter _productAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {

            //Maybe unnnesscarydyryryryr
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            //Create view
            View view = inflater.Inflate(Resource.Layout.ProductDialogView, container, true);



            StringBuilder sb = new StringBuilder();

            foreach (var item in _product.PriceElements) {
                sb.Append(item.name + " for " + item.price + System.Environment.NewLine);
            }

            view.FindViewById<TextView>(Resource.Id.productName).Text = _product.name;
            //view.FindViewById<ImageView>(Resource.Id.productImage). //TODO: N�r vi har billeder og bruge?
            view.FindViewById<TextView>(Resource.Id.productPrices).Text = sb.ToString();

            //_productAdapter.�

            //Test textssssss
            /*_textSomething = view.FindViewById<TextView>(Resource.Id.textView1);
            _textSomething.Text = _product.name;*/
            
            //Button test dismiss
            //_buttonDismiss = view.FindViewById<Button>(Resource.Id.Button_Dismiss);
            //_buttonDismiss.Click += Button_Dismiss_Click;
            
            return view;
        }
        public void PassDataToFrag(Product product, ProductAdapter productAdapter)
        {
            _product = product;
            _productAdapter = productAdapter;
        }

        public override void OnResume()
        {

            // Auto Size based on content
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            // No background behind the view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.DarkGray));

            base.OnResume();
        }
        private void Button_Dismiss_Click(object sender, EventArgs e)
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
        }
    }
}