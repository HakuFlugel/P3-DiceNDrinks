using System.Drawing;
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


        public FormProgressBar() {
            probar.Maximum = 200;
            ///Size = new Size(220, 50);
            Size = new Size(256, 50);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(probar);

        }
        public void addToProbar() {
            probar.Value++;
        }

        public void setProbarValue(int value) {
            probar.Maximum = value;
        }
    }
}
