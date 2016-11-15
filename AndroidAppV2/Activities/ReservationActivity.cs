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
    public class ReservationActivity : Activity, SeekBar.IOnSeekBarChangeListener
    {
        public bool State = true; //checks if the user has made any changes

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReservationLayout);
            // Create your application here


            //TODO: Her skal laves forbindelse med serveren så man kan indsende reservationer
            SeekBar sb = FindViewById<SeekBar>(Resource.Id.seekBar1);
            DateTime chosenDateTime = DateTime.Now;
            TextView dateDisplay = FindViewById<TextView>(Resource.Id.textView1);
            Button dateSelectButton = FindViewById<Button>(Resource.Id.button1);
            Button timeSelectButton = FindViewById<Button>(Resource.Id.button2);
            Button updateButton = FindViewById<Button>(Resource.Id.button3);

            dateSelectButton.Click += delegate
            {
                DatePickerFragment dfrag = DatePickerFragment.NewInstance(delegate(DateTime date)
                {
                    chosenDateTime = InsertDateTime(date, chosenDateTime);
                    State = false;
                });
                dfrag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            timeSelectButton.Click += delegate
            {
                TimePickerFragment tfrag = TimePickerFragment.NewInstance(delegate(DateTime time)
                {
                    chosenDateTime = InsertDateTime(chosenDateTime,time);
                    State = false;
                });
                tfrag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            updateButton.Click += delegate
            {
                dateDisplay.Text = chosenDateTime.ToLongDateString() + " " + chosenDateTime.ToLongTimeString();
            };

            sb.Max = 20;
            sb.SetOnSeekBarChangeListener(this);

        }

        private void SendData(DateTime datetime, int paritcipants)
        {
            //todo: send reservation here
        }

        private DateTime InsertDateTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day,time.Hour,time.Minute,time.Second);
        }

        public override void OnBackPressed()
        {
            if (!State)
            {
                AlertDialog.Builder exitApp = new AlertDialog.Builder(this);
                exitApp.SetMessage(Resource.String.exitReservation);
                exitApp.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { base.OnBackPressed(); });
                exitApp.SetNegativeButton(Resource.String.no, (senderAlert, args) => { /*Scratch Ass*/ });

                Dialog exit = exitApp.Create();

                exit.Show();
            }
            else
            base.OnBackPressed();
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (fromUser)
            {
                FindViewById<TextView>(Resource.Id.textView2).Text = $"{seekBar.Progress}";
                System.Diagnostics.Debug.WriteLine($"seekbar progress: {seekBar.Progress}");
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            System.Diagnostics.Debug.WriteLine("Tracking changes.");
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            System.Diagnostics.Debug.WriteLine("Stopped tracking changes.");
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
            DatePickerFragment frag = new DatePickerFragment {_dateSelectedHandler = onDateSelected};
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
            TimePickerFragment frag = new TimePickerFragment {_timeSelectedHandler = onDateSelected};
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the current time as the default values for the picker
            DateTime currently = DateTime.Now;
            // Create a new instance of TimePickerDialog and return it
            TimePickerDialog dialog = new TimePickerDialog(Activity, 
                                                            this, 
                                                            currently.Hour, 
                                                            currently.Minute, 
                                                            true);

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