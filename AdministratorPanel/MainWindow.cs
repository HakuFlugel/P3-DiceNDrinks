﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class MainWindow : Form {

        private List<AdminTabPage> tabs = new List<AdminTabPage>();
        private ReservationController reservationController;
        private FormProgressBar probar = new FormProgressBar();
        private TabControl tabControl = new TabControl();

        public MainWindow() {
            //tab name
            Text = "Dice 'n Drinks";
            probar.setProbarValue(35);                          //For progress bar, set the value. If you add or remove a probar.addToProbar(); then change this number!!!!!!!!!1! IT IS HARDCODED!
            probar.Show();

            StartPosition = FormStartPosition.CenterScreen;
            
            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;
            
            tabControl.Dock = DockStyle.Fill;
            probar.addToProbar();                               //For progress bar. 1

            reservationController = new ReservationController();
            reservationController.load();

            tabs.AddRange(new AdminTabPage[]{
                new ReservationTab(reservationController,probar),
                new ProductsTab(probar),
                new GamesTab(probar),
                new EventsTab(probar)
            });

            probar.addToProbar();                               //For progress bar. 2

            tabControl.Controls.AddRange(tabs.ToArray());
            probar.addToProbar();                               //For progress bar. 3

            Controls.Add(tabControl);
            probar.addToProbar();                               //For progress bar. 4

            Activate();

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

        [STAThread] // do we still need thisssss.
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
