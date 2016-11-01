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
        }

        private static Color smartRGB(params int[] par)
        {
            for (int i = 0; i < par.Length; i++)
            {
                par[i] = par[i] > 255 ? 255 : par[i] < 0 ? 0 : par[i];
            }


            return Color.FromArgb(par[0], par[1], par[2]);
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
            EventHandler mouseEnter = (sender, ev) =>
            {
                Color b = e.Control.BackColor;
                //e.Control.ForeColor = e.Control.BackColor;
                e.Control.BackColor = smartRGB(b.R - 20, b.G - 20, b.B - 20);
            };
            EventHandler mouseLeave = (sender, ev) =>
            {
                Color b = e.Control.BackColor;
                //e.Control.ForeColor = e.Control.BackColor;
                e.Control.BackColor = smartRGB(b.R + 20, b.G + 20, b.B + 20);
            };

            e.Control.Click -= clickEvent;
            e.Control.Click += clickEvent;
            e.Control.ControlAdded -= controladded;
            e.Control.ControlAdded += controladded;

            e.Control.MouseEnter -= mouseEnter;
            e.Control.MouseEnter += mouseEnter;

            e.Control.MouseLeave -= mouseLeave;
            e.Control.MouseLeave += mouseLeave;

            controladded(e.Control, e);
        }
    }
}
