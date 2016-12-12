using System.Windows.Forms;

namespace AdministratorPanel {
    public class EventList : TableLayoutPanel {
        public EventList() {
            Name = "EventList";
            ColumnCount = 1;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            AutoScroll = true;
        }

        public EventsTab EventsTab {
            get {
                throw new System.NotImplementedException();
            }

            set {
            }
        }
    }
}
