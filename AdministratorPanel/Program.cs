using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace AdministratorPanel {
    class Program {

        private Form form;


        public Program() {
            form = new Form();
            form.Width = 1400;
            form.Height = 1000;

            

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

           TabPage[] tabs = {
                    new CalendarTab(),
                    new ProductsTab(),
                    new GamesTab(),
                    new EventsTab()
            };


            cp.Controls.AddRange(tabs);
            form.Controls.Add(cp);

            form.Activate();
            form.Show();


        }

        private void Start() {
            Application.Run();
        }




















        static void Main(string[] args) {

            Program p = new Program();
            p.Start();
        }


    }
}
