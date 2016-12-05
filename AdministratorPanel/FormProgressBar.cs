using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class FormProgressBar : Form {

       private ProgressBar probar = new ProgressBar() {
            Style = ProgressBarStyle.Continuous,
            Dock = DockStyle.Fill,
            Minimum = 0,
            Step = 1,
            Value = 0,

            //Width = 200,
       };
//        private NiceTransparentLabel probarLoading = new NiceTransparentLabel() {
//            Text = "Loading",
//            Font = new Font("Arial", 12, FontStyle.Regular),
//            AutoSize = true,
//            Dock = DockStyle.Right,
//            ForeColor = Color.Black,
//        };

        public FormProgressBar() {
            Size = new Size(256, 50);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(probar);
            //probar.Controls.Add(probarLoading);
            Show();
        }
        public void addToProbar() {
            probar.Value++;

        }
        public void setProbarValue(int value) {
            probar.Maximum = value;
        }
    }
}
