using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            messageBox.Text = sTitle;
            messageBox.Controls.AddRange(control.ToArray());

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
