using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.Res;
using Android.OS;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Debug = System.Diagnostics.Debug;

namespace CrossPlatformApp
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
		    Xamarin.Forms.RelativeLayout relative = new Xamarin.Forms.RelativeLayout
		    {
		        Padding = 0,
		    };
            Image imageButton1 = new Image
            {
                Source = "Top_Left_Games.png",
                Aspect = Aspect.AspectFill,
            };
		    Image imageButton2 = new Image
		    {
                Source = "Top_Right_Menu.png",
                Aspect = Aspect.AspectFill,
            };
            Image imageButton3 = new Image
            {
                Source = "Bottom_Left_Reservation.png",
                Aspect = Aspect.AspectFill,
            };
            Image imageButton4 = new Image
            {
                Source = "Bottom_Right_Events.png",
                Aspect = Aspect.AspectFill,
            };
            /*AspectChange(imageButton1, relative);
            AspectChange(imageButton2, relative);
            AspectChange(imageButton3, relative);
            AspectChange(imageButton4, relative);*/
            relative.Children.Add(imageButton1,
                Constraint.Constant(0)
                );
            relative.Children.Add(imageButton2,
                Constraint.RelativeToParent(parent => parent.Width/2)
                );
            relative.Children.Add(imageButton3,
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Height / 2)
                );
            relative.Children.Add(imageButton4,
                Constraint.RelativeToParent(parent => parent.Width / 2),
                Constraint.RelativeToParent(parent => parent.Height / 2)
                );

            var tgr = new TapGestureRecognizer();
		    tgr.Tapped += OnLabelClicked;
            imageButton1.GestureRecognizers.Add(tgr);

            relative.BackgroundColor = Color.Black;

            MainPage = new ContentPage {
				Content = new Frame {
                    Padding = 0,
                    Content = relative
				}
                
			};
		}

	    private void OnLabelClicked(object s,EventArgs e)
	    {

	        Debug.WriteLine("object: " + s);
            Debug.WriteLine("event: " + e);
	    }

	    private void AspectChange(Image image, Xamarin.Forms.RelativeLayout layout)
	    {
	        image.WidthRequest = layout.Width;
            image.HeightRequest = layout.Height;
	    }

	    protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
