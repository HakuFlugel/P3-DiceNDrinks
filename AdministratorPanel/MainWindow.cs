using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class MainWindow : Form {

        List<AdminTabPage> tabs = new List<AdminTabPage>();

        public ReservationController reservationController;

        public MainWindow()
        {

            Text = "Dice 'n Drinks";

            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            //cp.AutoSize = true;

            reservationController = new ReservationController();
            reservationController.load();

            tabs.AddRange(new AdminTabPage[]{
                new ReservationTab(reservationController),
                new ProductsTab(),
                new GamesTab(),
                new EventsTab()
            });

            cp.Controls.AddRange(tabs.ToArray());
            Controls.Add(cp);
            
            Activate();
            Show();

        }

        public void Start() {

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
        public static void Main(string[] args) {

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
