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
            Dock = DockStyle.Fill;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            this.MakeSuperClickable((s, ev) => { this.BackColor = Color.Red; });


            Controls.Add(new Label() { Text = "Sex with hem? ADOPTERET OG EN ULYKKE", AutoSize = true});
            Controls.Add(new Label() { Font = new Font("Arial", 30), Text = "Fuck hem", AutoSize = true});
            
        }
    }
}
