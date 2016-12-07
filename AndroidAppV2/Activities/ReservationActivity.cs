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

namespace AndroidAppV2.Activities {
    [Activity(Label = "Reservations", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReservationActivity : Activity, SeekBar.IOnSeekBarChangeListener {
        private bool _state = true; //checks if the user has made any changes
        private DateTime _chosenDateTime = DateTime.Now;
        private int _userId;
        private Reservation _res;
        private bool _data; // checks if the user already has made a reservation
        private Button _dateSelectButton;
        private Button _timeSelectButton;

        private bool Data {
            get { return _data; }
            set {
                _data = value;
                Button acceptingButton = FindViewById<Button>(Resource.Id.acceptButton);
                acceptingButton.Text = "Change Reservation";
            }
        }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReservationLayout);

            SeekBar sb = FindViewById<SeekBar>(Resource.Id.seekBar1);
            _dateSelectButton = FindViewById<Button>(Resource.Id.dateButton);
            _timeSelectButton = FindViewById<Button>(Resource.Id.timeButton);
            Button acceptingButton = FindViewById<Button>(Resource.Id.acceptButton);


            AndroidShared.LoadData(this, "TheUserReservationID.json", out _userId);
            AndroidShared.LoadData(this, "VirtualServerReservation.json", out _res);

            if (_userId == default(int))
                _state = false;

            //Using Random because we have no server to request from (method implemention)? TODO: SERVER
            if (_userId == 0) {
                Random random = new Random();

                _userId = random.Next(0, 100);
            }

            if (_res == null) {
                _res = new Reservation();
                sb.Progress = 0;
            }
            else {
                Data = true;
                sb.Progress = _res.numPeople - 1;
                _chosenDateTime = _res.time;
                _dateSelectButton.Text = _res.time.ToString("dd. MMMMM, yyyy");
                _timeSelectButton.Text = _res.time.ToString("HH:mm");
                FindViewById<TextView>(Resource.Id.inviteesNum).Text = _res.numPeople.ToString();
                FindViewById<EditText>(Resource.Id.nameEdit).Text = _res.name;
                FindViewById<EditText>(Resource.Id.phoneNumEdit).Text = _res.phone;
                FindViewById<EditText>(Resource.Id.emailEdit).Text = _res.email;
                FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations state: Awaiting answer";
            }
            _dateSelectButton.Click += delegate {
                DatePickerFragment dfrag = DatePickerFragment.NewInstance(delegate (DateTime date) {
                    _chosenDateTime = InsertDateTime(date, _chosenDateTime);
                    _dateSelectButton.Text = _chosenDateTime.ToString("dd. MMMMM, yyyy");

                });
                dfrag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            _timeSelectButton.Click += delegate {
                TimePickerFragment tfrag = TimePickerFragment.NewInstance(delegate (DateTime time) {
                    _chosenDateTime = InsertDateTime(_chosenDateTime, time);
                    _timeSelectButton.Text = _chosenDateTime.ToString("HH:mm");
                });
                tfrag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            acceptingButton.Click += delegate {
                _res.numPeople = sb.Progress + 1;
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

        private void SendData(Reservation res) {
            if (res.name == "") {
                ErrorDialog("Please write a name");
                return;
            }
            if (res.phone == "" && res.email == "") {
                ErrorDialog("You have to specify either a phonenumber or an email");
                return;
            }
            if (res.email != "") {
                try {
                    EmailCheck(res.email);
                }
                catch (Java.Lang.Exception en) {
                    ErrorDialog(en.Message);
                    return;
                }
            }
            int i;
            if (res.phone.Length != 8 || !int.TryParse(res.phone, out i)) {
                ErrorDialog("a phone number has to be eight digits: e.g. 12345678");
                return;
            }
            if (_dateSelectButton.Text == "DATE" || _timeSelectButton.Text == "TIME") {
                ErrorDialog("Specify a date and time you want to place your reservation");
                return;
            }


            //Saving locally instead of server saving
            string json = JsonConvert.SerializeObject(res);
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD";
            string filename = Path.Combine(path, "VirtualServerReservation.json");

            File.WriteAllText(filename, json);

            string json2 = JsonConvert.SerializeObject(res.id);
            string filename2 = Path.Combine(path, "TheUserReservationID.json");

            File.WriteAllText(filename2, json2);

            AlertDialog.Builder resSucces = new AlertDialog.Builder(this);
            if (Data) {
                resSucces.SetMessage("Your reservation has been updated, and are awating a answer!");
                resSucces.SetTitle("Reservation updated");
            }
            else {
                resSucces.SetMessage("Your reservation has been created, and are awating a answer!");
                resSucces.SetTitle("Reservation sent");
            }
            _state = true;
            resSucces.SetPositiveButton(Resource.String.ok, (senderAlert, args) => { /*Do Nothing*/ });
            resSucces.Show();
            FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations state: Awaiting answer";

            Data = true;

        }

        private void ErrorDialog(string message) {
            AlertDialog.Builder error = new AlertDialog.Builder(this);
            error.SetMessage(message);
            error.SetTitle("Error");
            error.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { /*Do Nothing*/ });
            error.Show();
        }

        private static void EmailCheck(string email) {
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

            foreach (char ch in emailParts[0].Where(ch => !char.IsLetterOrDigit(ch) && !validLocalSymbols.Contains(ch))) {
                throw new Java.Lang.Exception($"Email address local-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{ validLocalSymbols }\"");
            }

            foreach (char ch in emailParts[1].Where(ch => !char.IsLetterOrDigit(ch) && !validDomainSymbols.Contains(ch))) {
                throw new Java.Lang.Exception($"Email address domain-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{ validDomainSymbols }\"");
            }
        }

        private static DateTime InsertDateTime(DateTime date, DateTime time) {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        public override void OnBackPressed() {
            if (!_state) {
                AlertDialog.Builder exitApp = new AlertDialog.Builder(this);
                exitApp.SetMessage(Resource.String.exitReservation);
                exitApp.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { base.OnBackPressed(); });
                exitApp.SetNegativeButton(Resource.String.no, (senderAlert, args) => { /*Do Nothing*/ });

                Dialog exit = exitApp.Create();

                exit.Show();
            }
            else
                base.OnBackPressed();
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser) {
            if (fromUser) {
                FindViewById<TextView>(Resource.Id.inviteesNum).Text = $"{seekBar.Progress + 1} people";
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar) {

        }

        public void OnStopTrackingTouch(SeekBar seekBar) {

        }
    }


    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener {
        // ReSharper disable once InconsistentNaming
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        private Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected) {
            DatePickerFragment frag = new DatePickerFragment { _dateSelectedHandler = onDateSelected };
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState) {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            dialog.DatePicker.MinDate = new Java.Util.Date().Time - 1000;

            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }


    public class TimePickerFragment : DialogFragment, TimePickerDialog.IOnTimeSetListener {
        // ReSharper disable once InconsistentNaming
        public static readonly string TAG = "X:" + typeof(TimePickerFragment).Name.ToUpper();

        private Action<DateTime> _timeSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<DateTime> onDateSelected) {
            TimePickerFragment frag = new TimePickerFragment { _timeSelectedHandler = onDateSelected };
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState) {
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

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute) {
            DateTime selectedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourOfDay, minute, 0);
            selectedTime = Round(selectedTime, new TimeSpan(0, 15, 0));
            Log.Debug(TAG, selectedTime.ToLongDateString());
            _timeSelectedHandler(selectedTime);
        }

        private static DateTime Round(DateTime dateTime, TimeSpan interval) {
            long halfIntervelTicks = ((interval.Ticks + 1) >> 1);

            return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
        }
    }
}