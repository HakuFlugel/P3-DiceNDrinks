using System.Windows.Forms;
using System.Drawing;


namespace AdministratorPanel {
    public class Program : Form {

        public Program() {
            
  
            //form.AutoSize = true;
            //form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;

            TabPage[] tabs = {
                new CalendarTab(),
                new ProductsTab(),
                new GamesTab(this),
                new EventsTab(this)
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
