using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AdministratorPanel {
    abstract class FansyPopupBox {
        private Form messageBox = new Form();
        public string sTitle = "";
        public string sName = "";
        public string sDescription = "";
        public string sID = "";
        public string sImage = "";
        public List<Control> control = new List<Control>();

        public FansyPopupBox() {
            
            messageBox.FormBorderStyle = FormBorderStyle.FixedSingle;
            messageBox.Width = 800;
            messageBox.Height = 500;
            control.Add(new Control());


        }
        
        public void openMessageBox() {

            defaultButtons();
            messageBox.Text = sTitle;
            messageBox.Controls.AddRange(control.ToArray());
           
        }

        private void defaultButtons() {
            Point startLoacttion = new Point(12, 393);

            for(int i = 0; i < 2; i++) {
                Button[] buttons = { new Button(), new Button(), new Button() };
                buttons[0].Click += cancle; buttons[1].Click += Delete;
                foreach(var item in buttons) {
                    item.Size = new Size(134, 48);
                    item.Location = new Point(startLoacttion.X, startLoacttion.Y);
                    startLoacttion.X += 318;
                    
                }
                

            }
        }

        private void Delete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void cancle(object sender, EventArgs e) {
            messageBox.Close();
        }
    }
}

/*
  public bool isGames = false;
        //shared
        

        //for games
        public int gYear;
        public string gGenre = "";
        public int gMinTime;
        public int gMaxTime;
        public int gMinPlayers;
        public int gMaxPlayers;

        //for products
        public int pRice;


     */
