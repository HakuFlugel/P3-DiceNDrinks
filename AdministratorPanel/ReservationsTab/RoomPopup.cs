﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel
{
    public class RoomPopup : FancyPopupBox // TODO: different base; don't need delete button
    {
        private ReservationController reservationController;
        private DataGridView roomGrid;
        private DataTable roomTable = new DataTable();

        public RoomPopup(ReservationController reservationController) : base(canDelete: false)
        {
            this.reservationController = reservationController;

        }

        protected override void OnShown(EventArgs e)
        {
            roomTable.Columns.Add("Room");
            roomTable.Columns.Add("Seats");

            for (int i = 0; i < reservationController.rooms.Count; i++)
            {
                DataRow dr = roomTable.NewRow();

                dr["Room"] = reservationController.rooms[i].name;
                dr["Seats"] = reservationController.rooms[i].seats;

                roomTable.Rows.Add(dr);
            }

            roomGrid.DataSource = roomTable;

        }

        protected override void save(object sender, EventArgs e)
        {
            List<Room> rooms = new List<Room>();

            for (int i = 0; i < roomTable.Rows.Count; i++)
            {
                try
                {
                    if (roomTable.Rows[i]["room"] == DBNull.Value && roomTable.Rows[i]["seats"] == DBNull.Value)
                    {
                        continue;
                    }

                    string roomName = roomTable.Rows[i]["room"].ToString();
                    int seats = int.Parse(roomTable.Rows[i]["seats"].ToString());

                    rooms.Add(new Room{name = roomName, seats = seats});

                }
                catch (Exception ex)
                {
                    Console.WriteLine("saveroom " + ex.Message);
                    MessageBox.Show("Failed to parse a row" + ex.Message);
                    return;
                }
            }

            foreach (var existingroom in reservationController.rooms)
            {
                if (!rooms.Any(r => r.id == existingroom.id))
                {
                    reservationController.removeRoom(existingroom);
                }
            }

            foreach (var room in rooms)
            {

                if (reservationController.rooms.Any(r => r.name == room.name))
                {
                    reservationController.changeRoom(room);
                }
                else
                {
                    reservationController.addRoom(room);
                }
            }

            //reservationController.rooms = rooms;

            base.save(sender, e);
        }

        protected override Control CreateControls()
        {
            roomGrid = new DataGridView();
            roomGrid.AutoSize = true;

            return roomGrid;
        }

        protected override void delete(object sender, EventArgs e)
        {

            MessageBox.Show(this, "The delete button is not supposed to be here");
        }


    }
}