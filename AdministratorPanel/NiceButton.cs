using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class Buttom : TableLayoutPanel
    {
        private Color normalColor;
        private Color hoverColor;

        public Color bgColor
        {
            set
            {
                normalColor = smartRGB(value.R - 10, value.G - 10, value.B - 10);
                hoverColor = smartRGB(value.R + 10, value.G + 10, value.B + 10);
                BackColor = normalColor;
            }
        }

        public Buttom()
        {

        }

        private void makeClickable(Control c)
        {
            EventHandler onClick = (s, e)    => { this.OnClick(e); };
            EventHandler mouseEnter = (s, e) => { this.OnMouseEnter(e); };
            EventHandler mouseLeave = (s, e) => { this.OnMouseLeave(e); };
            ControlEventHandler controlAdded = (s, e) => { this.makeClickable(e.Control); };

            c.Click -= onClick;
            c.Click += onClick;

            c.MouseEnter -= mouseEnter;
            c.MouseEnter += mouseEnter;

            c.MouseLeave -= mouseLeave;
            c.MouseLeave += mouseLeave;

            c.ControlAdded -= controlAdded;
            c.ControlAdded += controlAdded;

            foreach (Control control in c.Controls)
            {
                makeClickable(control);
            }

        }

        protected override void OnControlAdded(ControlEventArgs e)
        {

            makeClickable(e.Control);

            base.OnControlAdded(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            BackColor = hoverColor;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            BackColor = normalColor;

            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.Focus();
            base.OnClick(e);
        }

        private static Color smartRGB(params int[] par)
        {
            for (int i = 0; i < par.Length; i++)
            {
                par[i] = par[i] > 255 ? 255 : par[i] < 0 ? 0 : par[i];
            }


            return Color.FromArgb(par[0], par[1], par[2]);
        }
    }
}