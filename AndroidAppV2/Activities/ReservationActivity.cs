using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Android;
using Android.Content.PM;
using Newtonsoft.Json;
using System.IO;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Reservation", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class ReservationActivity : Activity, SeekBar.IOnSeekBarChangeListener
    {
        public bool State = true; //checks if the user has made any changes
        private DateTime _chosenDateTime = DateTime.Now;
        private int _userID;
        private Reservation res;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReservationLayout);
            // Create your application here

            //TODO: Her skal laves forbindelse med serveren så man kan indsende reservationer
            SeekBar sb = FindViewById<SeekBar>(Resource.Id.seekBar1);
            TextView dateText = FindViewById<TextView>(Resource.Id.dateText);
            TextView timeText = FindViewById<TextView>(Resource.Id.timeText);
            Button dateSelectButton = FindViewById<Button>(Resource.Id.dateButton);
            Button timeSelectButton = FindViewById<Button>(Resource.Id.timeButton);
            Button acceptingButton = FindViewById<Button>(Resource.Id.acceptButton);


            LoadID();

            //Using Random because we have no server to request from (method implemention)?
            if (_userID == 0) {
                Random random = new Random();

                _userID = random.Next(0, 100);
            }

            LoadData();
            
            if (res == null) {
                res = new Reservation();
            }
            else {
                sb.Progress = res.numPeople;
                _chosenDateTime = res.time;
                dateText.Text = res.time.ToString("dd. MMMMM, yyyy");
                timeText.Text = res.time.ToString("HH:mm");
                FindViewById<EditText>(Resource.Id.nameEdit).Text = res.name;
                FindViewById<EditText>(Resource.Id.phoneNumEdit).Text = res.phone;
                FindViewById<EditText>(Resource.Id.emailEdit).Text = res.email;
            }
            dateSelectButton.Click += delegate
            {
                DatePickerFragment dfrag = DatePickerFragment.NewInstance(delegate(DateTime date)
                {
                    _chosenDateTime = InsertDateTime(date, _chosenDateTime);
                    State = false;
                    dateText.Text = _chosenDateTime.ToString("dd. MMMMM, yyyy");

                });
                dfrag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            timeSelectButton.Click += delegate
            {
                TimePickerFragment tfrag = TimePickerFragment.NewInstance(delegate(DateTime time)
                {
                    _chosenDateTime = InsertDateTime(_chosenDateTime,time);
                    State = false;
                    timeText.Text = _chosenDateTime.ToString("HH:mm");
                });
                tfrag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            acceptingButton.Click += delegate 
            {
                res.numPeople = sb.Progress;
                res.time = _chosenDateTime;
                res.name = FindViewById<EditText>(Resource.Id.nameEdit).Text;
                res.phone = FindViewById<EditText>(Resource.Id.phoneNumEdit).Text;
                res.email = FindViewById<EditText>(Resource.Id.emailEdit).Text;
                res.created = DateTime.Now;

                //SERVER TODO: Request ID method? - Why we have random atm.
                res.id = _userID;
                SendData(res);
            };

            sb.Max = 20;
            sb.SetOnSeekBarChangeListener(this);

        }
        private void LoadID() {
            string input;
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(path + "/TheUserReservationID.json")) {
                return;
            }
            var filename = Path.Combine(path, "TheUserReservationID.json");

            input = File.ReadAllText(filename);

            if (input != null) {
                _userID = JsonConvert.DeserializeObject<int>(input);
            }
        }

        private void LoadData() {
            //SERVER TODO: Should get reservation based on ID.
            //Loading locally instead
            string input;
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(path + "/VirtualServerReservation.json")) {
                return;
            }
            var filename = Path.Combine(path, "VirtualServerReservation.json");

            input = File.ReadAllText(filename);

            if (input != null) {
                res = JsonConvert.DeserializeObject<Reservation>(input);
            }
        }

        private void SendData(Reservation res)
        {
            //SERVER TODO: Should send reservation connected to the ID server gave the user.
            //Saving locally instead
            var json = JsonConvert.SerializeObject(res);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "VirtualServerReservation.json");

            File.WriteAllText(filename, json);

            var json2 = JsonConvert.SerializeObject(res.id);
            var filename2 = Path.Combine(path, "TheUserReservationID.json");

            File.WriteAllText(filename2, json2);

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
                FindViewById<TextView>(Resource.Id.inviteesNum).Text = $"{seekBar.Progress}";
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

        //TODO: OVERRIDE OnBackPressed to Pause instead of destroy activity
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