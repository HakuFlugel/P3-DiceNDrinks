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
            //form.Width = 1400;
            //form.Height = 1000;
            form.AutoSize = true;
            //form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            form.AutoScaleMode = AutoScaleMode.Dpi;

            TabControl cp = new TabControl();
            cp.Dock = DockStyle.Fill;
            cp.Font = new Font(cp.Font.OriginalFontName, cp.Font.Size*2); //todo: look at this font size

  

           TabPage[] tabs = {
                    new TabPage("Calendar"),
                    new ProductCategoryTab(),
                    new TabPage("Games"),
                    new TabPage("Events")
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
