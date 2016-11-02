using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Shared;

namespace AdministratorPanel {
    class EventItem : NiceButton {

        public string name;
        public string description;
        public DateTime startDate;
        public DateTime endDate;

        public EventItem(Event evnt) {
            this.name = evnt.name;
            this.description = evnt.description;
            this.startDate = evnt.startDate;
            this.endDate = evnt.endDate;
            
            RowCount = 1;
            ColumnCount = 2;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            //this.MakeSuperClickable((s, ev) => { this.BackColor = Color.Red; });

            TableLayoutPanel lft = new TableLayoutPanel();
            lft.Dock = DockStyle.Top;
            lft.RowCount = 2;
            lft.ColumnCount = 1;
            lft.AutoSize = true;

            lft.Controls.Add(new Label { Text = name, Font = new Font("Arial", 20), Dock = DockStyle.Top, AutoSize = true });
            lft.Controls.Add(new Label { Text = description, Dock = DockStyle.Top, Width = 750 });

            Controls.Add(lft);
            Controls.Add(new Label { Text = "\n" + startDate.ToString() + "\n" + endDate.ToString(), Dock = DockStyle.Right, AutoSize = true });


        }
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
        }
    }
}
