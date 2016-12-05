using System;
using System.Collections.Generic;
using System.Linq;

// TODO: reservation af lokale osv. : Vi har List<int> med de rum der har her. Har List<int> på hver dag, hvor de gennem en checkbox eller lignende kan tilføje/fjerne en sådan reservation

namespace Shared
{
    public class ReservationController : ControllerBase
    {

        public int totalSeats;
        public List<Room> rooms = new List<Room>();
        public List<CalendarDay> reservationsCalendar = new List<CalendarDay>();

        public event EventHandler ReservationUpdated;

        //TODO: make sure it is pending if from user

        public void addReservation(Reservation reservation) {
            reservation.id = getRandomID();
            reservation.created = DateTime.Now;

            addToDay(reservation);

            //ReservationAdded?.Invoke(this, new AddReservationEventArgs(reservation));
            ReservationUpdated?.Invoke(this, EventArgs.Empty);

            checkIfAutoAccept(reservation, findDay(reservation.time));
        }
        //TODO: make sure it is pending if from user
        public void updateReservation(Reservation reservation)
        {
            Reservation oldReservation =
                reservationsCalendar.SelectMany(cd => cd.reservations).First(r => r.id == reservation.id);

            reservation.created = oldReservation.created;

            removeFromDay(oldReservation);

            addToDay(reservation);
            if(reservation.time.Date != oldReservation.time.Date)
                checkIfAutoAccept(reservation, findDay(reservation.time));

            ReservationUpdated?.Invoke(this, EventArgs.Empty);

        }

        public void removeReservation(Reservation reservation) {

            removeFromDay(reservation);

            //ReservationRemoved?.Invoke(this, new RemoveReservationEventArgs(reservation));
            ReservationUpdated?.Invoke(this, EventArgs.Empty);
        }

        private Random rand = new Random();

        public ReservationController(string path = "data/") : base(path)
        {
        }

        private int getRandomID()
        {
            int id;

            do id = rand.Next(); while (reservationsCalendar.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        private void addToDay(Reservation reservation) {

            CalendarDay resDay = findDay(reservation.time);
            
            resDay.reservations.Add(reservation);
            resDay.calculateReservedSeats();
            //resDay.reservedSeats += reservation.numPeople;
            
        }

        public CalendarDay findDay(DateTime date) {
            CalendarDay resDay = reservationsCalendar.FirstOrDefault(o => o.theDay.Date == date.Date);
            if (resDay == null) {
                resDay = new CalendarDay() { theDay = date.Date };
                reservationsCalendar.Add(resDay);
            }
            return resDay;
        }
        public bool checkIfRemove(CalendarDay day) {
            if (day.theDay < DateTime.Today.AddDays(-1) || !day.isLocked && day.reservations.Count < 1
                 && day.defaultAcceptMaxPeople == day.autoAcceptMaxPeople
                 && day.defaultAcceptPresentage == day.acceptPresentage)

                return true;
            else
                return false;
        }

        private void removeFromDay(Reservation reservation) {
            CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);
            resDay.reservations.Remove(reservation);
            resDay.calculateReservedSeats(); 

            if(checkIfRemove(resDay)) 
                reservationsCalendar.Remove(resDay);
            
            //resDay.reservedSeats -= reservation.numPeople;
        }

        public void addRoom(Room room) {
            rooms.Add(room);
            calculateSeats();

            foreach (var day in reservationsCalendar)
            {
                day.calculateSeats(this);
            }
        }

        public void removeRoom(Room room)
        {
            rooms.Remove(room);
            calculateSeats();

            foreach (var day in reservationsCalendar) {
                day.unreserveRoom(this, room);
                //day.roomsReserved.Remove(room);
                //day.calculateSeats(this);
            }
        }

        public void changeRoom(Room oldroom, Room room) {
            rooms[rooms.IndexOf(oldroom)] = room;
            calculateSeats();

            foreach (var day in reservationsCalendar) {
                int roomindex = day.roomsReserved.IndexOf(oldroom);
                if (roomindex < 0) continue;

                day.roomsReserved[roomindex] = room;
                day.calculateSeats(this);
            }
        }

        private void calculateSeats()
        {
            totalSeats = rooms.Sum(r => r.seats);
        }

        public override void save()
        {
            saveFile("reservationsCalendar", reservationsCalendar);
            saveFile("rooms", rooms);
        }
        public void checkIfAutoAccept(Reservation reservation, CalendarDay resDay) {
         
            
            //Console.WriteLine(resDay == null? "DAY IS NULL" : resDay.isAutoaccept.ToString() + " " + resDay.acceptPresentage.ToString() + " <= " + "(" + resDay.reservedSeats.ToString() + "+" + reservation.numPeople.ToString() + ")*100 / " + resDay.numSeats.ToString());

            int reservedSeats = 0;

            if(resDay != null ) {
                if (resDay.numSeats == 0)
                    resDay.calculateSeats(this);
                    resDay.calculateReservedSeats();
//                foreach (var item in resDay.reservations.Where(x => x.state == Reservation.State.Accepted))
//                    reservedSeats += item.numPeople;
            }

            if (resDay != null && resDay.isLocked && reservation.state != Reservation.State.Denied) {
                reservation.state = Reservation.State.Denied;
                Console.WriteLine("Denied");


            } else if ((resDay == null && reservation.numPeople <= 5
             || (!resDay.isLocked
             && resDay.autoAcceptMaxPeople >= reservation.numPeople
             && resDay.isAutoaccept //maybe
             && resDay.acceptPresentage >= (resDay.reservedSeats + reservation.numPeople) * 100 / resDay.numSeats))
             && reservation.state != Reservation.State.Accepted) {

                reservation.state = Reservation.State.Accepted;
                Console.WriteLine("Resevation: " + reservation.name + " Accepted");
                
                

            } else 
                return;
            updateReservation(reservation);



        }


        public override void load()
        {
            reservationsCalendar = loadFile<CalendarDay>("reservationsCalendar");
            rooms = loadFile<Room>("rooms");
        }

    }
}