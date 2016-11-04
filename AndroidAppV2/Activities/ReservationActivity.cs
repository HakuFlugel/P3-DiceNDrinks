using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Android;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Reservation")]
    public class ReservationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.ReservationLayout);
            // Create your application here

            TextView dateDisplay = FindViewById<TextView>(Resource.Id.textView1);
            Button dateSelectButton = FindViewById<Button>(Resource.Id.button1);
            dateSelectButton.Click += delegate
            {
                DatePickerFragment dfrag = DatePickerFragment.NewInstance(delegate(DateTime time)
                {
                    dateDisplay.Text = time.ToLongDateString();
                });
                dfrag.Show(FragmentManager, DatePickerFragment.TAG);
            };
            TextView timeDisplay = FindViewById<TextView>(Resource.Id.textView2);
            Button timeSelectButton = FindViewById<Button>(Resource.Id.button2);

            timeSelectButton.Click += delegate
            {
                TimePickerFragment tfrag = TimePickerFragment.NewInstance(delegate(DateTime time)
                {
                    timeDisplay.Text = time.ToLongTimeString();
                });
                tfrag.Show(FragmentManager, TimePickerFragment.TAG);
            };

        }
    }


    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month,
                                                           currently.Day);
            dialog.DatePicker.MinDate = new Java.Util.Date().Time -1000;
            
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }


    public class TimePickerFragment : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        public static readonly string TAG = "X:" + typeof(TimePickerFragment).Name.ToUpper();

        Action<DateTime> _timeSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            TimePickerFragment frag = new TimePickerFragment();
            frag._timeSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the current time as the default values for the picker
            DateTime currently = DateTime.Now;
            // Create a new instance of TimePickerDialog and return it
            TimePickerDialog dialog = new TimePickerDialog(Activity, this, currently.Hour, currently.Minute, true);

            return dialog;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime selectedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourOfDay, minute, 0);
            Log.Debug(TAG, selectedTime.ToLongDateString());
            _timeSelectedHandler(selectedTime);
        }
    }
}