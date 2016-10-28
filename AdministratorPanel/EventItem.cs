using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AdministratorPanel {
    class EventItem : TableLayoutPanel {

        public string name { get; set; }
        

        public EventItem() {
            RowCount = 1;
            ColumnCount = 2;
            BackColor = Color.LightGray;
            //Dock = DockStyle.None;
            //this.MakeSuperClickable()
        }
    }
}
