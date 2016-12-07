using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
using System.Media;

namespace AdministratorPanel {
    /*abstract*/
    public abstract class FancyPopupBox : Form {

        private TableLayoutPanel container = new TableLayoutPanel() {
            RowCount = 2,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };

        private Panel p = new Panel() {
            Height = 48,
            Dock = DockStyle.Bottom,
            BackColor = Color.Transparent,
            Padding = new Padding(8)
        };

        private Button deletebutton = new Button() {
            Text = "Delete",
            Name = "delete",
            Dock = DockStyle.Left
        };

        private Button cancelButton = new Button() {
            Text = "Cancel",
            Name = "cancel",
            Dock = DockStyle.Right,
        };

        private Button saveButton = new Button() {
            Text = "Save",
            Name = "save",
            Dock = DockStyle.Right,
        };

        protected Padding labelPadding = new Padding(5, 0, 5, 0);
        protected Padding otherPadding = new Padding(5, 0, 5, 20);
        public bool hasBeenChanged = false;

        public FancyPopupBox() {
            MaximizeBox = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FormBorderStyle = FormBorderStyle.Fixed3D;
           
            Controls.Add(container);
            container.Controls.Add(CreateControls());

            container.Controls.Add(p);
           
            p.Controls.Add(deletebutton);
            deletebutton.Click += this.delete;

            cancelButton.Click += this.cancel;
            p.Controls.Add(cancelButton);

            saveButton.Click += this.save;
            p.Controls.Add(saveButton);

        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            switch (e.CloseReason) {
                case CloseReason.UserClosing:
                    SystemSounds.Question.Play();
                    //TODO: see product on close for messagebox text
                    if (hasBeenChanged && DialogResult.No ==
                        MessageBox.Show("Are you sure? Everything unsaved will be lost.",
                            "About to close", MessageBoxButtons.YesNo)) {

                        e.Cancel = true;
                    }

                    break;
            }
            base.OnFormClosing(e);
        }

        protected abstract Control CreateControls();

        protected virtual void save(object sender, EventArgs e) {

            hasBeenChanged = false;
            Close();
        }

        protected abstract void delete(object sender, EventArgs e);

        private void cancel(object sender, EventArgs e) {
            Close();
        }
    }
}