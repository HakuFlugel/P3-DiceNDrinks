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
                buttons[0].Click += cancle; buttons[1].Click += Delete; buttons[2].Click += Save;
                foreach(var item in buttons) {
                    item.Size = new Size(134, 48);
                    item.Location = new Point(startLoacttion.X, startLoacttion.Y);
                    startLoacttion.X += 318;
                    
                }

            }
        }

        abstract public void Save(object sender, EventArgs e);

        abstract public void Delete(object sender, EventArgs e);

        private void cancle(object sender, EventArgs e) {
            messageBox.Close();
        }
    }
    class ProductPopupbox : FansyPopupBox {
        //for products
        public int pRice;
        ProductPopupbox() {

        }

        public override void Delete(object sender, EventArgs e) {
        }

        public override void Save(object sender, EventArgs e) {
        }
    }

    class GamesPopupbox : FansyPopupBox {
        //for games
        public int gYear;
        public string gGenre = "";
        public int gMinTime;
        public int gMaxTime;
        public int gMinPlayers;
        public int gMaxPlayers;

        GamesPopupbox() {

        }

        public override void Delete(object sender, EventArgs e) {
        }

        public override void Save(object sender, EventArgs e) {
        }
    }


}






/*
  public bool isGames = false;
        //shared
        

        

        


     */
