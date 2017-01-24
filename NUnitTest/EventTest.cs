using NUnit.Framework;
using AdministratorPanel;
using Shared;

namespace NUnitTest {
    [TestFixture]
    public class EventTest {
        Event testEvent = new Event();
        FormProgressBar probar = new FormProgressBar();
        EventsTab eventsTap;
        EventItem eventItem;

        public EventTest() {
            eventsTap = new EventsTab(probar);
            testEvent.id = 1;
            testEvent.name = "Weston Wensday";
            testEvent.description = "Something weston related on wensday";

            eventsTap.EventList.Add(testEvent);
            eventsTap.makeItems();
        }

        [Test]
        public void EventItemCreationTest() {
            eventItem = new EventItem(eventsTap, testEvent);
            Assert.True(eventItem.name == testEvent.name
                        && eventItem.description == testEvent.description);
        }
    }
}
