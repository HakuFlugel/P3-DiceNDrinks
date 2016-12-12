using System;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class NiceTextBox : TextBox {
        //TODO: override text getter or similar to prevent getting watermark
        private bool waterMarkActive = true;
        private string _waterMark = "";
        public string waterMark {
            get { return _waterMark; }
            set {
                _waterMark = value;
                Text = value;
            }
        }

        protected override void OnGotFocus(EventArgs e) {
            if (Text == waterMark && waterMarkActive) {
                Text = "";
            }
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e) {
            if (Text == "") {
                Text = waterMark;
                waterMarkActive = true;
            } else {
                waterMarkActive = false;
            }

            base.OnLostFocus(e);
        }

        // TODO: Untested, WIP
        private bool _clearable = false;
        private Button clearButton;

        public bool clearable {
            get { return _clearable; }
            set {
                _clearable = value;
                if (value && clearButton == null) {
                    clearButton = new Button();
                    clearButton.Width = clearButton.Height;
                    clearButton.Text = "X";
                    clearButton.Dock = DockStyle.Right;
                    clearButton.MouseHover += (s, e) => {
                        this.Cursor = Cursors.Hand;
                    };
                    clearButton.MouseLeave += (s, e) => {
                        this.Cursor = Cursors.IBeam;
                    };

                    clearButton.Click += (sender, ev) => {
                        Text = Focused ? "" : waterMark;
                        waterMarkActive = true;
                    };

                    Controls.Add(clearButton);
                } else if (!value && clearButton != null) {
                    Controls.Remove(clearButton);
                    clearButton = null;
                }
            }
        }
    }

   
}
