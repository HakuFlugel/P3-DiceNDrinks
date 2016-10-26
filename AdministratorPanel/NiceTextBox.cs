using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministratorPanel {
    class NiceTextBox : TextBox {
        public string waterMark = "";
        public NiceTextBox(string waterMark) {
            this.waterMark = waterMark;
            Text = waterMark;
        }
        protected override void OnGotFocus(EventArgs e) {
            if(waterMark != "") {
                Text = "";
            }
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e) {
            if(Text == "") {
                Text = waterMark;
            }
            base.OnLostFocus(e);
        }
    }
}
