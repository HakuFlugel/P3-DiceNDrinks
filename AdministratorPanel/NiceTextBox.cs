using System;
using System.Windows.Forms;

namespace AdministratorPanel {
    class NiceTextBox : TextBox
    {
        private bool waterMarkActive = true;
        private string _waterMark = "";
        public string waterMark
        {
            get { return _waterMark; }
            set
            {
                _waterMark = value;
                Text = value;
            }
        }

        protected override void OnGotFocus(EventArgs e) {
            if(Text == waterMark && waterMarkActive) {
                Text = "";
            }
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e) {
            if(Text == "") {
                Text = waterMark;
                waterMarkActive = true;
            }
            else
            {
                waterMarkActive = false;
            }

            base.OnLostFocus(e);
        }


        // TODO: Untested, WIP
        private bool _clearable = false;
        private Button clearButton;

        public bool clearable
        {
            get { return _clearable; }
            set
            {
                _clearable = value;
                if (value && clearButton == null)
                {
                    clearButton = new Button();
                    clearButton.Width = clearButton.Height;
                    clearButton.Text = "X";
                    clearButton.Dock = DockStyle.Right;

                    clearButton.Click += (sender, ev) =>
                    {
                        Text = Focused ? "" : waterMark;
                        waterMarkActive = true;
                    };

                    Controls.Add(clearButton);
                } else if (!value && clearButton != null)
                {
                    Controls.Remove(clearButton);
                    clearButton = null;
                }
            }
        }
    }

    class NiceDropDownBox : ComboBox {
        private bool _userCanWrite = false;
        public bool userCanWrite {
            get { return _userCanWrite; }
            set {
                _userCanWrite = value;
                if(value) 
                    DropDownStyle = ComboBoxStyle.DropDownList;
                else 
                    DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        private bool _defaultSelection = false;
        public bool defaultSeletion {
            get {
                return _defaultSelection;
            }
            set {
                _defaultSelection = value;
                if (value)
                    TabIndex = 0;
            }
        }

        public NiceDropDownBox() {
            DrawMode = DrawMode.Normal;
            DropDownWidth = 250;

        }
    }
}
