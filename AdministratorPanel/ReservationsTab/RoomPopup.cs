using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Shared;

namespace AdministratorPanel {
    public class RoomPopup : FancyPopupBox
    {
        private ReservationController reservationController;
        private CalendarDay day;

        public ListView roomsListView = new ListView()
        {
            View = View.Details,
            LabelEdit = true,
            AllowColumnReorder = true,
            CheckBoxes = true,
            FullRowSelect = true,
            GridLines = true,
            Dock = DockStyle.Left,
            Sorting = SortOrder.Ascending,
            Width = 200,
        };

        private Button modifyRoomsButton = new Button()
        {
            Text = "Modify Rooms",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };


        public RoomPopup(ReservationController reservationController, CalendarDay day) : base(canDelete: false)
        {
            this.day = day;
            this.reservationController = reservationController;

            modifyRoomsButton.Click += (sender, args) => {

                ModifyRoomPopup rp = new ModifyRoomPopup(reservationController);

                rp.FormClosed += (o, eventArgs) =>
                {
                    update();
                };

                rp.Show();
            };

            update();
        }

        public void update()
        {

            roomsListView.Clear();

            roomsListView.Columns.Add("test", -2);

            foreach (var room in reservationController.rooms)
            {
                roomsListView.Items.Add(new ListViewItem($"{room.name} : {room.seats}seat{(room.seats != 1 ? "s" : "")}")
                {
                    Name = room.name,
                    Checked = day.roomsReserved.FirstOrDefault(r => r.name == room.name) != null
                });
            }
        }

        protected override void save(object sender, EventArgs e) {
            List<Room> rooms = new List<Room>();

//            for (int i = 0; i < roomsListView.Items.Count; i++)
//            {
//                if (roomsListView.Items[i].Checked)
//                {
//                    rooms.Add(reservationController.rooms[i]);
//                }
//            }

                foreach (ListViewItem item in roomsListView.Items) // NAME field is somehow null
                {
                    if (item.Checked)
                    {
                        rooms.Add(reservationController.rooms.First(r => r.name == item.Name));
                    }
                }

            string response = ServerConnection.sendRequest("/submitRoomReserved.aspx",
                new NameValueCollection() {
                    {"Rooms", JsonConvert.SerializeObject(rooms)},
                    {"Date", JsonConvert.SerializeObject(day.theDay.Date)}
                }
            );
            Console.WriteLine(response);

            if (response != "success")
            {
                Console.WriteLine("failed to submit room reservations");
                return;
            }

            reservationController.submitReservedRooms(rooms, day.theDay.Date);



            base.save(sender, e);
        }

        protected override Control CreateControls()
        {
            TableLayoutPanel tlp = new TableLayoutPanel()
            {
                RowCount = 2,
                ColumnCount = 1,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
                AutoSize = true,
            };

            tlp.Controls.Add(roomsListView);
            tlp.Controls.Add(modifyRoomsButton);

            return tlp;
        }

        protected override void delete(object sender, EventArgs e) {

            NiceMessageBox.Show(this, "The delete button is not supposed to be here");
        }


    }
}