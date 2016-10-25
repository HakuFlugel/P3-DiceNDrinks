using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace AdministratorPanel {
    public class Program : Form {

        public Program() {

            MinimumSize = new Size(480, 270);

            Width = 960;
            Height = 540;


            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            TabPage[] tabs = {
                new CalendarTab(),
                new ProductsTab(),
                new GamesTab(),
                new EventsTab()
            };


            cp.Controls.AddRange(tabs);
            Controls.Add(cp);


            Activate();
            Show();

        }

        private void Start() {

            DoubleBuffered = true;
            Application.Run(this);
        }


        static void Main(string[] args) {

            Program p = new Program();
            p.Start();
        }
    }
}
