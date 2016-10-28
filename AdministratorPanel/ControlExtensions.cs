using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministratorPanel {
    public static class ControlExtensions {

        public static void MakeSuperClickable(this Control control, EventHandler clickEvent) { // TODO: hover events too?
            ControlEventArgs cev = new ControlEventArgs(control);

            clickEvent += (sender, ev) => { ((Control)sender).Focus(); };

            OnControlAdded(cev, clickEvent);
        }

        private static void OnControlAdded(ControlEventArgs e, EventHandler clickEvent) {
            ControlEventHandler controladded = (sender, ev) => {

                foreach (Control control in e.Control.Controls) {
                    ControlEventArgs cev = new ControlEventArgs(control);
                    OnControlAdded(cev, clickEvent);
                }
            };

            e.Control.Click -= clickEvent;
            e.Control.Click += clickEvent;
            e.Control.ControlAdded -= controladded;
            e.Control.ControlAdded += controladded;

            controladded(e.Control, e);
        }
    }
}
