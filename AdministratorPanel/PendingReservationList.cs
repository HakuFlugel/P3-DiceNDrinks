using System.Windows.Forms;

namespace AdministratorPanel
{
    public class PendingReservationList : FlowLayoutPanel
    {

        public PendingReservationList()
        {
            Text = "Something";
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;
            AutoScroll = true;

            for (int i = 0; i < 100; i++)
            {
                Button button = new Button();
                button.Text = "66/6 6666\nLars | 42\nJens | 66";
                //button.Height = 666;
                button.Height = (int)(button.Height * 3 / 1.5);
                //button.Anchor = AnchorStyles.Left & AnchorStyles.Right & AnchorStyles.Top;
                //button.Dock = DockStyle.Fill;
                //button.AutoSizeMode = AutoSizeMode.GrowOnly;

                Controls.Add(button);
                button.Width = button.Parent.ClientSize.Width - 20;
                TextBox t = new TextBox();


            }
        }

    }
}