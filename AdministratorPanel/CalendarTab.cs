using System;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class CalendarTab : TabPage
    {
        private Calendar calendar;

        public CalendarTab() {
            Text = "Calendar";

            calendar = new Calendar();
            Controls.Add(calendar);


        }
    }
}
