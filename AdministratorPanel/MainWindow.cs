using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class MainWindow : Form {

        private List<AdminTabPage> tabs = new List<AdminTabPage>();
        private ReservationController reservationController;
        private FormProgressBar probar = new FormProgressBar();
        private TabControl tabControl = new TabControl();

        private Button refreshButton = new Button()
        {
            Text = "↻",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            //Anchor = AnchorStyles.Top | AnchorStyles.Right,

        };

        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel()
        {
            ColumnCount = 1,
            RowCount = 2,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };

        private FlowLayoutPanel topFlowPanel = new FlowLayoutPanel()
        {
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };

        public MainWindow() {
            //tab name
            Text = "Dice 'n Drinks";
            probar.setProbarValue(35);                          //For progress bar, set the value. If you add or remove a probar.addToProbar(); then change this number!!!!!!!!!1! IT IS HARDCODED!
            probar.Show();

            StartPosition = FormStartPosition.CenterScreen;
            
            AutoScaleMode = AutoScaleMode.Dpi;

            MinimumSize = new Size(960, 540);
            Width = 960;
            Height = 540;
            Padding = Padding.Empty;
            
            tabControl.Dock = DockStyle.Fill;
            probar.addToProbar();                               //For progress bar. 1

            reservationController = new ReservationController();
            reservationController.load();

            tabs.AddRange(new AdminTabPage[]{
                new ReservationTab(reservationController,probar),
                new ProductsTab(probar),
                new GamesTab(probar),
                new EventsTab(probar)
            });

            probar.addToProbar();                               //For progress bar. 2

            tabControl.Controls.AddRange(tabs.ToArray());
            probar.addToProbar();                               //For progress bar. 3

            refreshButton.Click += (sender, args) =>
            {
                foreach (var tab in tabs)
                {
                    tab.download();
                }
            };
            topFlowPanel.Controls.Add(refreshButton);
            tableLayoutPanel.Controls.Add(topFlowPanel);

            tableLayoutPanel.Controls.Add(tabControl);

            Controls.Add(tableLayoutPanel);
            probar.addToProbar();                               //For progress bar. 4

            Activate();

            probar.addToProbar();                               //For progress bar. 5
            Show();
            
            probar.Close();
        }

        private void Start() {
            DoubleBuffered = true;
            Application.Run(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            foreach (var tab in tabs) {
                tab.Save();
            }
            reservationController.save();
        }

        [STAThread] // do we still need thisssss.
        private static void Main(string[] args) {

            //ReservationController reservationController = new ReservationController();
            //reservationController.load();
            //...
            //...
            //...

            ServerConnection.ip = "172.25.11.113"; // TODO: get this from somewhere

            MainWindow p = new MainWindow();

            p.Start();
        }
    }
}
