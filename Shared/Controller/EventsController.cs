using System;
using System.Collections.Generic;

namespace Shared
{
    public class EventsController : ControllerBase
    {

        List<Event> events = new List<Event>();

        public event EventHandler<UpdateEventEventArgs> EventUpdated;
        public class UpdateEventEventArgs
        {
            public Event newEvent;
            public Event oldEvent;

            public UpdateEventEventArgs(Event oldEvent, Event newEvent)
            {
                this.oldEvent = oldEvent;
                this.newEvent = newEvent;
            }
        }

        public void addEvent(Event newEvent)
        {
            newEvent.id = new Random().Next(); // TODO: unique id

            events.Add(newEvent);

            EventUpdated?.Invoke(this, new UpdateEventEventArgs(null, newEvent));
        }


        public void updateEvent(Event oldEvent, Event newEvent)
        {
            //newProduct.created = oldProduct.created;
            newEvent.id = oldEvent.id; // TODO: needed?

            events[events.FindIndex(e => e == oldEvent)] = newEvent;

            EventUpdated?.Invoke(this, new UpdateEventEventArgs(oldEvent, newEvent));

        }

        public void removeEvent(Event oldEvent)
        {

            events.Remove(oldEvent); //TODO: make sure this uses id to compare

            EventUpdated?.Invoke(this, new UpdateEventEventArgs(oldEvent, null));
        }

        public override void save()
        {
            saveFile("data/reservationsCalendar.json", events);
        }

        public override void load()
        {
            events = loadFile<Event>("data/reservationsCalendar.json");
        }

    }
}