using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

using Java.Lang;

using Shared;

namespace AndroidAppV2.ListDialogFragments {
    internal class ProductDialogFragment : DialogFragment {
        private Product _product;
        private Context _context;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle) {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            //Create view
            View view = inflater.Inflate(Resource.Layout.ProductDialogView, container, true);

            StringBuilder sb = new StringBuilder();

            foreach (var item in _product.PriceElements) {
                sb.Append(item.name + " for " + item.price + " kr,-" + System.Environment.NewLine);
            }
            AndroidShared an = new AndroidShared();
            view.FindViewById<TextView>(Resource.Id.productName).Text = _product.name;
            int[] sizes = { 150, 150 }; //placeholder as we do not have larger images
            an.GetImagesFromSD(_context, _product.image, view, Resource.Id.productImage, sizes);
            view.FindViewById<TextView>(Resource.Id.productPrices).Text = sb.ToString();

            return view;
        }
        public void PassDataToFrag(Product product, Context context) {
            _product = product;
            _context = context;
        }

        public override void OnResume() {

            // Auto Size based on content
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            // No background behind the view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.DarkGray));

            base.OnResume();
        }
    }
}