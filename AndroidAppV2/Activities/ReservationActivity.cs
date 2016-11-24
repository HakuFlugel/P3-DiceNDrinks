using System;
using System.Linq;
using Shared;

using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using Newtonsoft.Json;
using System.IO;
using Android.Content.PM;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Reservation", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReservationActivity : Activity, SeekBar.IOnSeekBarChangeListener
    {
        private bool _state = false; //checks if the user has made any changes
        private DateTime _chosenDateTime = DateTime.Now;
        private int _userId;
        private Reservation _res;
        private bool _data; // checks if the user already has made a reservation

        private bool Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Button acceptingButton = FindViewById<Button>(Resource.Id.acceptButton);
                acceptingButton.Text = "�ndrer Reservation";
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReservationLayout);
            // Create your application here


            SeekBar sb = FindViewById<SeekBar>(Resource.Id.seekBar1);
            Button dateSelectButton = FindViewById<Button>(Resource.Id.dateButton);
            Button timeSelectButton = FindViewById<Button>(Resource.Id.timeButton);
            Button acceptingButton = FindViewById<Button>(Resource.Id.acceptButton);


            AndroidShared.LoadSavedData(this, "TheUserReservationID.json", out _userId);
            AndroidShared.LoadSavedData(this, "VirtualServerReservation.json", out _res);
            //LoadID();
            //LoadData();

            //Using Random because we have no server to request from (method implemention)?
            if (_userId == 0) {
                Random random = new Random();

                _userId = random.Next(0, 100);
            }

            if (_res == null) {
                _res = new Reservation();
            }
            else
            {
                Data = true;
                sb.Progress = _res.numPeople;
                _chosenDateTime = _res.time;
                dateSelectButton.Text = _res.time.ToString("dd. MMMMM, yyyy");
                timeSelectButton.Text = _res.time.ToString("HH:mm");
                FindViewById<TextView>(Resource.Id.inviteesNum).Text = _res.numPeople.ToString();
                FindViewById<EditText>(Resource.Id.nameEdit).Text = _res.name;
                FindViewById<EditText>(Resource.Id.phoneNumEdit).Text = _res.phone;
                FindViewById<EditText>(Resource.Id.emailEdit).Text = _res.email;
                FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations stadie: Afventer svar";
            }
            dateSelectButton.Click += delegate
            {
                DatePickerFragment dfrag = DatePickerFragment.NewInstance(delegate(DateTime date)
                {
                    _chosenDateTime = InsertDateTime(date, _chosenDateTime);
                    dateSelectButton.Text = _chosenDateTime.ToString("dd. MMMMM, yyyy");

                });
                dfrag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            timeSelectButton.Click += delegate
            {
                TimePickerFragment tfrag = TimePickerFragment.NewInstance(delegate(DateTime time)
                {
                    _chosenDateTime = InsertDateTime(_chosenDateTime,time);
                    timeSelectButton.Text = _chosenDateTime.ToString("HH:mm");
                });
                tfrag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            acceptingButton.Click += delegate 
            {
                _res.numPeople = sb.Progress;
                _res.time = _chosenDateTime;
                _res.name = FindViewById<EditText>(Resource.Id.nameEdit).Text;
                _res.phone = FindViewById<EditText>(Resource.Id.phoneNumEdit).Text;
                _res.email = FindViewById<EditText>(Resource.Id.emailEdit).Text;
                _res.created = DateTime.Now;

                _res.id = _userId;
                SendData(_res);
            };
            
            sb.Max = 20;
            sb.SetOnSeekBarChangeListener(this);

            

        }

        /*private void LoadID() {
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
        }*/


        private void SendData(Reservation res)
        {
            if (res.phone == "" && res.email == "") {
                AlertDialog.Builder errorEmailPhone = new AlertDialog.Builder(this);
                errorEmailPhone.SetMessage("You need to input a phone number or a email");
                errorEmailPhone.SetTitle("Error");
                errorEmailPhone.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { /*Scratch Ass*/ });
                errorEmailPhone.Show();
                return;
            }
            if (res.email != "") {
                try {
                    EmailCheck(res.email);
                }
                catch (Java.Lang.Exception en) {
                    AlertDialog.Builder typoEmail = new AlertDialog.Builder(this);
                    typoEmail.SetMessage(en.Message);
                    typoEmail.SetTitle("Typo Error");
                    typoEmail.SetPositiveButton(Resource.String.ok, (senderAlert, args) => { /*Scratch Ass*/ });
                    typoEmail.Show();
                    return;
                }
            }
            

            //Saving locally instead of server saving
            var json = JsonConvert.SerializeObject(res);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "VirtualServerReservation.json");

            File.WriteAllText(filename, json);

            var json2 = JsonConvert.SerializeObject(res.id);
            var filename2 = Path.Combine(path, "TheUserReservationID.json");

            File.WriteAllText(filename2, json2);

            AlertDialog.Builder resSucces = new AlertDialog.Builder(this);
            if (Data)
            {
                resSucces.SetMessage("Din reservation er blevet opdateret! Og venter nu p� at blive godkendt!");
                resSucces.SetTitle("Reservation opdateret");
            }
            else
            {
                resSucces.SetMessage("Din reservation er blevet sendt! Og venter nu p� at blive godkendt!");
                resSucces.SetTitle("Reservation sendt");
            }
            _state = true;
            resSucces.SetPositiveButton(Resource.String.ok, (senderAlert, args) => { /*Scratch Ass*/ });
            resSucces.Show();
            FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations stadie: Afventer svar";
            
            Data = true;

        }

        public void EmailCheck(string email) {
            // Email typo check stuff

            const string validLocalSymbols = "!#$%&'*+-/=?^_`{|}~"; // !#$%&'*+-/=?^_`{|}~      quoted og evt. escaped "(),:;<>@[]
            const string validDomainSymbols = ".-";

            string[] emailParts = email.Split('@');

            if (emailParts.Length != 2)
                throw new Java.Lang.Exception("Email address must contain exactly one '@'");
            if (emailParts[0].Length == 0 || emailParts[1].Length == 0) 
                throw new Java.Lang.Exception("Email address must contain something on both sides of '@'");
            
            if (emailParts[0][0] == '.' || emailParts[0][emailParts.Length - 1] == '.' || emailParts[1][0] == '.' || emailParts[1][emailParts.Length - 1] == '.')
                throw new Java.Lang.Exception("Email address local- or domain-part can't start or end with a '.'");

            if (!emailParts[1].Contains('.'))
                throw new Java.Lang.Exception("Email adress domain part must contain atleast 1 '.'. ie. @domain.tld");

            if (email.Contains(".."))
                throw new Java.Lang.Exception("Email address may not contain consecutive '.'s, ie. '..'.");

            foreach (char ch in emailParts[0]) {
                if (!Char.IsLetterOrDigit(ch) && !validLocalSymbols.Contains(ch))
                    throw new Java.Lang.Exception($"Email address local-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{ validLocalSymbols }\"");
            }

            foreach (var ch in emailParts[1]) {
                if (!Char.IsLetterOrDigit(ch) && !validDomainSymbols.Contains(ch))
                    throw new Java.Lang.Exception($"Email address domain-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{ validDomainSymbols }\"");
            }
        }

        private static DateTime InsertDateTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day,time.Hour,time.Minute,time.Second);
        }

        public override void OnBackPressed()
        {
            if (!_state)
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
                FindViewById<TextView>(Resource.Id.inviteesNum).Text = $"{seekBar.Progress} Person(er)";
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
        // ReSharper disable once InconsistentNaming
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
                                                           currently.Month - 1,
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
        // ReSharper disable once InconsistentNaming
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