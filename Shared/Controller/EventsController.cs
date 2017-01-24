using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class EventsController : ControllerBase
    {

        public List<Event> events = new List<Event>();

        public EventsController(string path = "data/") : base(path)
        {
        }

        public event EventHandler EventUpdated;


        public void addEvent(Event newEvent)
        {
            newEvent.id = new Random().Next();

            events.Add(newEvent);

            save();
            EventUpdated?.Invoke(this, EventArgs.Empty);
        }


        public void updateEvent(Event @event)
        {
            Event oldEvent =
                events.First(e => e.id == @event.id);

            if (oldEvent == null)
            {
                return;
            }

            events[events.FindIndex(e => e == oldEvent)] = @event;

            save();
            EventUpdated?.Invoke(this, EventArgs.Empty);

        }

        public void removeEvent(Event oldEvent)
        {

            events.Remove(oldEvent);

            save();
            EventUpdated?.Invoke(this, EventArgs.Empty);
        }

        public override void save()
        {
            saveFile("events", events);
        }

        public override void load()
        {
            events = loadFile<Event>("events");
        }

    }
}