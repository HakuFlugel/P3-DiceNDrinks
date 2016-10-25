using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace AdministratorPanel {
    class Program {
        private int width = 1200;
        private int height = 750;
        private Form form;


        public Program() {
            // For testing purpose only
            List<Games> games = new List<Games>();
            games.Add(new Games("AHZ2xB", "Secret Hitler", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("J8AkkS", "Dominion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("5ExgGS", "Small Worlds", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("TYE3sj", "Enter The Gundion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));


            form = new Form();
            form.Width = width;
            form.Height = height;

            

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            TabPage[] tabs = {
                    new CalendarTab(),
                    new ProductsTab(),
                    new GamesTab(form, games),
                    new EventsTab()
            };


            cp.Controls.AddRange(tabs);
            form.Controls.Add(cp);


            form.Activate();
            form.Show();
            form.Resize += Form_Resize;



        }

        private void Form_Resize(object sender, EventArgs e) {

            form.Width = width;
            form.Height = height;
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
