using System.Windows.Forms;
using System.Media;

namespace AdministratorPanel {
    public static class NiceMessageBox {
        public static DialogResult Show() {
            return Show("No message detected, but MessageBox");
        }

        public static DialogResult Show(IWin32Window owner, string BreadText) {
            return Show(owner, BreadText,"Attention!");
        }

        public static DialogResult Show(IWin32Window owner, string BreadText, string Caption) {
            return Show(owner, BreadText, Caption, MessageBoxButtons.OK);
        }

        public static DialogResult Show(IWin32Window owner, string BreadText, string Caption, MessageBoxButtons messageBoxButtons) {
            return Show(owner, BreadText, Caption, messageBoxButtons, MessageBoxIcon.Warning);
        }

        public static DialogResult Show(IWin32Window owner, string BreadText, string Caption, MessageBoxButtons messageBoxButtons, MessageBoxIcon Icon) {
            SystemSounds.Hand.Play();
            return MessageBox.Show(owner, BreadText, Caption, messageBoxButtons, Icon);
        }

        public static DialogResult Show(string BreadText) {
            return Show(BreadText, "Attention!");
        }

        public static DialogResult Show(string BreadText, string Caption) {
            return Show(BreadText, Caption, MessageBoxButtons.OK);
        }
        
        public static DialogResult Show(string BreadText, string Caption, MessageBoxButtons messageBoxButtons) {
            return Show(BreadText, Caption, messageBoxButtons, MessageBoxIcon.Warning);
        }

        public static DialogResult Show(string BreadText, string Caption, MessageBoxButtons messageBoxButtons, MessageBoxIcon icon) {
            SystemSounds.Hand.Play();
            return MessageBox.Show(BreadText, Caption, messageBoxButtons, icon);
        }
        
    }
}
