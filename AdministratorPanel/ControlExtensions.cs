/*
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public static class ControlExtensions
    {

        public static void MakeSuperClickable(this Control control, EventHandler clickEvent)
        {
            // TODO: hover events too?
            ControlEventArgs cev = new ControlEventArgs(control);

            clickEvent += (sender, ev) => { ((Control) sender).Focus(); };

            OnControlAdded(cev, clickEvent);

            EventHandler mouseEnter = (sender, ev) =>
            {
                Color b = control.BackColor;
                //e.Control.ForeColor = e.Control.BackColor;
                control.BackColor = smartRGB(b.R - 20, b.G - 20, b.B - 20);
            };
            EventHandler mouseLeave = (sender, ev) =>
            {
                Color b = control.BackColor;
                //e.Control.ForeColor = e.Control.BackColor;
                control.BackColor = smartRGB(b.R + 20, b.G + 20, b.B + 20);
            };

            control.MouseEnter -= mouseEnter;
            control.MouseEnter += mouseEnter;

            control.MouseLeave -= mouseLeave;
            control.MouseLeave += mouseLeave;
        }



        private static void OnControlAdded(ControlEventArgs e, EventHandler clickEvent)
        {
            ControlEventHandler controladded = (sender, ev) =>
            {

                foreach (Control control in e.Control.Controls)
                {
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
*/
