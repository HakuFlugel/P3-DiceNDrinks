using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Shared;

namespace AdministratorPanel {
    class EventItem : TableLayoutPanel {

        public EventItem(Event e) {
            RowCount = 1;
            ColumnCount = 2;
            BackColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            this.MakeSuperClickable((s, ev) => { this.BackColor = Color.Red; });


            Controls.Add(new Label() { Text = "Dinmor"});
        }
    }
}
