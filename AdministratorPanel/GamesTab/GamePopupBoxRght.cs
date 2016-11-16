using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class GamePopupBoxRght : TableLayoutPanel {

        private Xml api = new Xml();

        GamesList gameList;
        string seach = "";

        NiceTextBox seachBar = new NiceTextBox() {

            waterMark = "Type something to seach..",
            clearable = true,
            MinimumSize = new Size(200, 0),

            Margin = new Padding(20, 5, 20, 5),
        };



        public GamePopupBoxRght() {
            
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Dock = DockStyle.Fill;
            
        }


        



    
        
    }
}