using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class MainWindow : Form {

        private List<AdminTabPage> tabs = new List<AdminTabPage>();

        private ReservationController reservationController;
        private GamesController gamesController;

        private FormProgressBar probar = new FormProgressBar();


        public MainWindow() {
            probar.setProbarValue(37);                          //For progress bar, set the value. If you add or remove a probar.addToProbar(); then change this number!!!!!!!!!1! IT IS HARDCODED!
            
            Text = "Dice 'n Drinks";

            StartPosition = FormStartPosition.CenterScreen;
            
            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;
            

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;
            probar.addToProbar();                               //For progress bar. 1
             //cp.AutoSize = true;

            reservationController = new ReservationController();
            reservationController.load();
            gamesController = new GamesController();
            gamesController.load();

            tabs.AddRange(new AdminTabPage[]{
                new ReservationTab(reservationController,probar),
                new ProductsTab(probar),
                new GamesTab(gamesController, probar),
                new EventsTab(probar)
            });
            probar.addToProbar();                               //For progress bar. 2

            cp.Controls.AddRange(tabs.ToArray());
            probar.addToProbar();                               //For progress bar. 3

            Controls.Add(cp);
            probar.addToProbar();                               //For progress bar. 4

            Activate();

            probar.addToProbar();
            probar.addToProbar();                               //For progress bar. 5
            Show();
            
            probar.Close();
        }

        private void Start() {

            DoubleBuffered = true;
            Application.Run(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            foreach (var tab in tabs) {
                tab.Save();
            }
            reservationController.save();
        }

        [STAThread]
        private static void Main(string[] args) {

            //ReservationController reservationController = new ReservationController();
            //reservationController.load();
            //...
            //...
            //...


            MainWindow p = new MainWindow();
            //            {
            //                reservationController = reservationController
            //            };



            p.Start();
        }

    }
}
