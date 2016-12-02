using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class MainWindow : Form {

        private List<AdminTabPage> tabs = new List<AdminTabPage>();

        private ReservationController reservationController;

        private Form progressBar = new Form() {
            Size = new Size(220, 100),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            ControlBox = false,
            StartPosition = FormStartPosition.CenterScreen,
        };

        private ProgressBar probar = new ProgressBar() {
            Style = ProgressBarStyle.Continuous,
            Dock = DockStyle.Left,
            Minimum = 0,
            Step = 1,
            Maximum = 11,
            Value = 0,
            Width = 200
        };

        private Label probarLoading = new Label() {
            Text = "Loading",
            BackColor = Color.Transparent };

        public MainWindow() {

            progressBar.Show();
            progressBar.Controls.Add(probar);

            probar.Controls.Add(probarLoading);
            probar.Value++;
            progressBar.Focus();
            Text = "Dice 'n Drinks";

            StartPosition = FormStartPosition.CenterScreen;
            probar.Value++;
            progressBar.Focus();
            AutoScaleMode = AutoScaleMode.Dpi;
            probar.Value++;
            progressBar.Focus();
            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;

            probar.Value++;
            progressBar.Focus();

            progressBar.Focus();

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            probar.Value++;
            
            //cp.AutoSize = true;
            
            reservationController = new ReservationController();
            reservationController.load();

            probar.Value++;

            tabs.AddRange(new AdminTabPage[]{
                new ReservationTab(reservationController),
                new ProductsTab(),
                new GamesTab(),
                new EventsTab()
            });

            probar.Value++;

            cp.Controls.AddRange(tabs.ToArray());

            probar.Value++;

            Controls.Add(cp);

            probar.Value++;

            Activate();

            probar.Value++;

            
            Show();

            probar.Value++;

            progressBar.Close();
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
