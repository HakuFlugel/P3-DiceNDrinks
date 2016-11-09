using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class Calendar : MonthCalendar
    {


        public Calendar()
        {
            ShowTodayCircle = true; // Red circle
            ShowToday = true; // Text below calendar
            //calendar.Dock = DockStyle.Fill;

            MaxSelectionCount = 1; // You can select up to 7 days by default

            Font = new Font(Font.OriginalFontName, Font.Size*1.5f); // TODO: any better altenatives than this? Make our own calendar?
            
            //Padding = Padding.Empty;
            Margin = Padding.Empty;
            //Console.WriteLine(calendar.Font.Size); //1.5 = 12.375
            //Console.WriteLine(calendar.Font.SizeInPoints);  //1.5 = 12.375
        }
    }
}