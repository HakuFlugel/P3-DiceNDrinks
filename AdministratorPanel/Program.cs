using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace AdministratorPanel {
    public class Program : Form {

        List<AdminTabPage> tabs = new List<AdminTabPage>();

        public Program() {
            
  
            //AutoSize = true;
            //AutoSizeMode = AutoSizeMode.GrowOnly;
            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            //cp.AutoSize = true;

            tabs.AddRange(new AdminTabPage[]{
                new CalendarTab(),
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

        static void Main(string[] args) {

            Program p = new Program();
            p.Start();
        }
    }
}
