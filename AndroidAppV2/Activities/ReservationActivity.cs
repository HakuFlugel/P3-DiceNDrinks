using System;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Net;

using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

using Shared;
using Newtonsoft.Json;
using System.Globalization;

namespace AndroidAppV2.Activities
{
    [Activity(Label = "Reservations", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReservationActivity : Activity, SeekBar.IOnSeekBarChangeListener
    {
        private bool _state = true; //checks if the user has made any changes
        private DateTime _chosenDateTime = DateTime.Now;
        private int _userId;
        private Reservation _res;
        private bool _data; // checks if the user already has made a reservation
        private Button _dateSelectButton;
        private Button _timeSelectButton;
        private Button _acceptionButton;
        private Button _deleteButton;
        private EditText _nameEdit;
        private EditText _phoneNumEdit;
        private EditText _emailEdit;
        private TextView _invitees;
        private bool _hasConnectionToServer;

        private bool HasData
        {
            get { return _data; }
            set
            {
                _data = value;
                Button acceptingButton = FindViewById<Button>(Resource.Id.acceptButton);
                acceptingButton.Text = "Change Reservation";
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReservationLayout);

            SeekBar sb = FindViewById<SeekBar>(Resource.Id.seekBar1);
            _dateSelectButton = FindViewById<Button>(Resource.Id.dateButton);
            _timeSelectButton = FindViewById<Button>(Resource.Id.timeButton);
            _acceptionButton = FindViewById<Button>(Resource.Id.acceptButton);
            _deleteButton = FindViewById<Button>(Resource.Id.deleteButton);

            _nameEdit = FindViewById<EditText>(Resource.Id.nameEdit);
            _phoneNumEdit = FindViewById<EditText>(Resource.Id.phoneNumEdit);
            _emailEdit = FindViewById<EditText>(Resource.Id.emailEdit);

            _invitees = FindViewById<TextView>(Resource.Id.inviteesNum);


            _hasConnectionToServer = AndroidShared.CheckForInternetConnection();
            if (!_hasConnectionToServer) {
                if (LoadDataLocally()) {
                    sb.Progress = _res.numPeople - 1;
                    _chosenDateTime = _res.time;
                    _dateSelectButton.Text = _res.time.ToString("dd. MMMMM, yyyy");
                    _timeSelectButton.Text = _res.time.ToString("HH:mm");
                    FindViewById<TextView>(Resource.Id.inviteesNum).Text = _res.numPeople.ToString();
                    _nameEdit.Text = _res.name;
                    _phoneNumEdit.Text = _res.phone;
                    _emailEdit.Text = _res.email;
                    _userId = _res.id;
                    FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Gray);
                    FindViewById<TextView>(Resource.Id.textView1).Text = "Status unknown (no connection)";
                }
                _acceptionButton.Enabled = false;
                _deleteButton.Enabled = false;
                _dateSelectButton.Enabled = false;
                _timeSelectButton.Enabled = false;
                sb.Enabled = false;
                _nameEdit.Enabled = false;
                _phoneNumEdit.Enabled = false;
                _emailEdit.Enabled = false;
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).Visibility = ViewStates.Invisible;
                FindViewById<ImageView>(Resource.Id.roomStateImage).Visibility = ViewStates.Invisible;
                AndroidShared.ErrorDialog("Reservations can't be made at this moment (No connection to Dice N Drinks server)", this);
            }
            else {
                AndroidShared.LoadData(this, "TheUserReservationID.json", out _userId);

                if (_userId == default(int)) {
                    _state = false;
                }
                else {
                    _res = GetReservation();
                }

                if (_res == null) {
                    _res = new Reservation();
                    sb.Progress = 0;
                    _dateSelectButton.Text = DateTime.Now.ToString("dd. MMMMM, yyyy");
                    _timeSelectButton.Text = DateTime.Now.ToString("HH:mm");
                }
                else { 
                    HasData = true;
                    sb.Progress = _res.numPeople - 1;
                    _chosenDateTime = _res.time;
                    _dateSelectButton.Text = _res.time.ToString("dd. MMMMM, yyyy");
                    _timeSelectButton.Text = _res.time.ToString("HH:mm");
                    FindViewById<TextView>(Resource.Id.inviteesNum).Text = _res.numPeople.ToString();
                    _nameEdit.Text = _res.name;
                    _phoneNumEdit.Text = _res.phone;
                    _emailEdit.Text = _res.email;
                    _userId = _res.id;
                    _deleteButton.Enabled = true;
                    switch (_res.state)
                    {
                        case Reservation.State.Pending:
                            FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Yellow);
                            FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations state: Awaiting answer.";
                            break;
                        case Reservation.State.Accepted:
                            FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Green);
                            FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations state: Confirmed!";
                            break;
                        case Reservation.State.Denied:
                            FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Red);
                            FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations state: Denied!";
                            break;
                    }
                }
                getRoomSpace(_chosenDateTime);
            }
            

            _dateSelectButton.Click += delegate
            {
                DatePickerFragment dfrag = DatePickerFragment.NewInstance(delegate(DateTime date)
                {
                    _chosenDateTime = InsertDateTime(date, _chosenDateTime);
                    _dateSelectButton.Text = _chosenDateTime.ToString("dd. MMMMM, yyyy");
                    getRoomSpace(date);
                    
                });
                dfrag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            _timeSelectButton.Click += delegate
            {
                TimePickerFragment tfrag = TimePickerFragment.NewInstance(delegate(DateTime time)
                {
                    _chosenDateTime = InsertDateTime(_chosenDateTime, time);
                    _timeSelectButton.Text = _chosenDateTime.ToString("HH:mm");
                });
                tfrag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            _acceptionButton.Click += delegate
            {
                _res.numPeople = sb.Progress + 1;
                _res.time = _chosenDateTime;
                _res.name = _nameEdit.Text;
                _res.phone = _phoneNumEdit.Text;
                _res.email = _emailEdit.Text;
                _res.created = DateTime.Now;
                
                SendData(_res);
            };

            _deleteButton.Click += delegate 
            {
                DeleteReservation();
                sb.Progress = 0;
                _invitees.Text = "1";
                _timeSelectButton.Text = "TIME";
                _dateSelectButton.Text = "DATE";
                _nameEdit.Text = "";
                _phoneNumEdit.Text = "";
                _emailEdit.Text = "";
                _deleteButton.Enabled = false;
            };

            sb.Max = 19;
            sb.SetOnSeekBarChangeListener(this);
        }

        private void SendData(Reservation res)
        {
            if (res.name == "")
            {
                AndroidShared.ErrorDialog("Please write a name", this);
                return;
            }
            if (res.phone == "" && res.email == "")
            {
                AndroidShared.ErrorDialog("You have to specify either a phonenumber or an email", this);
                return;
            }
            if (res.email != "")
            {
                try
                {
                    EmailCheck(res.email);
                }
                catch (Java.Lang.Exception en)
                {
                    AndroidShared.ErrorDialog(en.Message, this);
                    return;
                }
            }
            int i;
            if (res.phone.Length != 8 || !int.TryParse(res.phone, out i))
            {
                AndroidShared.ErrorDialog("a phone number has to be eight digits: e.g. 12345678", this);
                return;
            }
            if (_dateSelectButton.Text == "DATE" || _timeSelectButton.Text == "TIME")
            {
                AndroidShared.ErrorDialog("Specify a date and time you want to place your reservation", this);
                return;
            }

            SaveDataLocally(_res);

            AlertDialog.Builder resSucces = new AlertDialog.Builder(this);
            if (HasData)
            {
                UpdateReservation();
                resSucces.SetMessage("Your reservation has been updated, and are awating an answer!");
                resSucces.SetTitle("Reservation updated");
            }
            else
            {
                AddReservation();
                resSucces.SetMessage("Your reservation has been created, and are awating an answer!");
                resSucces.SetTitle("Reservation sent");
            }
            _state = true;
            resSucces.SetPositiveButton(Resource.String.ok, (senderAlert, args) =>
            {
                /*Do Nothing*/
            });
            resSucces.Show();
            FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Yellow);
            FindViewById<TextView>(Resource.Id.textView1).Text = "Reservations state: Awaiting answer.";

            HasData = true;

        }
        private bool LoadDataLocally() {
            string input;
            var path = Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD";
            if (!File.Exists(path + "/VirtualServerReservation.json")) {
                return false;
            }
            var filename = Path.Combine(path, "VirtualServerReservation.json");

            input = File.ReadAllText(filename);

            if (input != null) {
                _res = JsonConvert.DeserializeObject<Reservation>(input);
                return true;
            }
            return false;
        }

        private void SaveDataLocally(Reservation res) {

            var json = JsonConvert.SerializeObject(res);
            var path = Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD";
            var filename = Path.Combine(path, "VirtualServerReservation.json");

            File.WriteAllText(filename, json);

            var json2 = JsonConvert.SerializeObject(res.id);
            var filename2 = Path.Combine(path, "TheUserReservationID.json");

            File.WriteAllText(filename2, json2);
        }

        private static void EmailCheck(string email)
        {
            // Email typo check stuff

            const string validLocalSymbols = "!#$%&'*+-/=?^_`{|}~";
                // !#$%&'*+-/=?^_`{|}~      quoted og evt. escaped "(),:;<>@[]
            const string validDomainSymbols = ".-";

            string[] emailParts = email.Split('@');

            if (emailParts.Length != 2)
                throw new Java.Lang.Exception("Email address must contain exactly one '@'");
            if (emailParts[0].Length == 0 || emailParts[1].Length == 0)
                throw new Java.Lang.Exception("Email address must contain something on both sides of '@'");

            if (emailParts[0][0] == '.' || emailParts[0][emailParts.Length - 1] == '.' || emailParts[1][0] == '.' ||
                emailParts[1][emailParts.Length - 1] == '.')
                throw new Java.Lang.Exception("Email address local- or domain-part can't start or end with a '.'");

            if (!emailParts[1].Contains('.'))
                throw new Java.Lang.Exception("Email adress domain part must contain atleast 1 '.'. ie. @domain.tld");

            if (email.Contains(".."))
                throw new Java.Lang.Exception("Email address may not contain consecutive '.'s, ie. '..'.");

            foreach (char ch in emailParts[0].Where(ch => !char.IsLetterOrDigit(ch) && !validLocalSymbols.Contains(ch)))
            {
                throw new Java.Lang.Exception(
                    $"Email address local-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{validLocalSymbols}\"");
            }

            foreach (char ch in emailParts[1].Where(ch => !char.IsLetterOrDigit(ch) && !validDomainSymbols.Contains(ch))
            )
            {
                throw new Java.Lang.Exception(
                    $"Email address domain-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{validDomainSymbols}\"");
            }
        }

        private static DateTime InsertDateTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        public override void OnBackPressed()
        {
            if (!_state)
            {
                AlertDialog.Builder exitApp = new AlertDialog.Builder(this);
                exitApp.SetMessage(Resource.String.exitReservation);
                exitApp.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { base.OnBackPressed(); });
                exitApp.SetNegativeButton(Resource.String.no, (senderAlert, args) =>
                {
                    /*Do Nothing*/
                });

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
                FindViewById<TextView>(Resource.Id.inviteesNum).Text = $"{seekBar.Progress + 1}";
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {

        }

        private void getRoomSpace(DateTime date) {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                new NameValueCollection
                {
                    {"Type", "Fullness"},
                    {"Day", date.ToLongDateString()}
                });

            string result = System.Text.Encoding.UTF8.GetString(resp);
            double value;
            if (!result.StartsWith("failed")) {
                try {
                    value = double.Parse(result, CultureInfo.InvariantCulture);
                }
                catch (Exception) {
                    Toast.MakeText(this, $"{result} Could not fetch how many reservation there is on that day", ToastLength.Long).Show();
                    value = 0;
                }
            }
            else {
                value = 0;
            }
            if (value < 60) {
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).SetTextColor(Android.Graphics.Color.Green);
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).Text = "Plenty of seats left";
                FindViewById<ImageView>(Resource.Id.roomStateImage).SetImageResource(Resource.Drawable.reserve_green);
            }
            else if (value < 85) {
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).SetTextColor(Android.Graphics.Color.Yellow);
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).Text = "Some seats left";
                FindViewById<ImageView>(Resource.Id.roomStateImage).SetImageResource(Resource.Drawable.reserve_yellow);
            }
            else {
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).SetTextColor(Android.Graphics.Color.Red);
                FindViewById<TextView>(Resource.Id.roomSpaceStateText).Text = "Few seats left";
                FindViewById<ImageView>(Resource.Id.roomStateImage).SetImageResource(Resource.Drawable.reserve_red);
            }
        }

        private void AddReservation()
        {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/submitReservation.aspx",
                new NameValueCollection
                {
                    {"Action", "add"},
                    {"Reservation", JsonConvert.SerializeObject(_res)}
                });

            string result = System.Text.Encoding.UTF8.GetString(resp);


            string[] newResult = result.Split(' ');
            int id;
            int.TryParse(newResult[1], out id);
            _res.id = id;

            string json = JsonConvert.SerializeObject(id);
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD";
            string filename = Path.Combine(path, "TheUserReservationID.json");
            File.WriteAllText(filename, json);

        }

        private void UpdateReservation()
        {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/submitReservation.aspx",
                new NameValueCollection
                {
                    {"Action", "update"},
                    {"Reservation", JsonConvert.SerializeObject(_res)}
                });
        }

        private Reservation GetReservation()
        {
            WebClient client = new WebClient();
            {
                byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                    new NameValueCollection
                    {
                        {"Type", "Reservations"},
                        {"ReservationID", _userId.ToString()}
                    });

                string result = System.Text.Encoding.UTF8.GetString(resp);
                try
                {
                    return JsonConvert.DeserializeObject<Reservation>(result);
                }
                catch (Exception)
                {
                    return default(Reservation);
                }

            }
        }

        private void DeleteReservation() {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/submitReservation.aspx",
                new NameValueCollection
                {
                    {"Action", "delete"},
                    {"Reservation", JsonConvert.SerializeObject(_res)}
                });

            string result = System.Text.Encoding.UTF8.GetString(resp);
            AndroidShared.ErrorDialog("The reservation has been: " + result, this);
        }

        protected override void OnResume() {
            base.OnResume();
            AndroidShared.Update();
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