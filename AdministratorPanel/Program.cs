using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace AdministratorPanel {
    public class Program : Form {

        List<AdminTabPage> tabs = new List<AdminTabPage>();

        public Program() {
            
            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            //cp.AutoSize = true;

            tabs.AddRange(new AdminTabPage[]{
                new ReservationsTab(),
                new ProductsTab(),
                new GamesTab(),
                new EventsTab()
            });

            cp.Controls.AddRange(tabs.ToArray());
            Controls.Add(cp);
            
            Activate();
            Show();

        }

        private void Start() {

            DoubleBuffered = true;
            Application.Run(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            foreach (var tab in tabs) {
                tab.Save();
            }
        }
        [STAThread]
        static void Main(string[] args) {

            Program p = new Program();
            p.Start();
        }
    }
}
