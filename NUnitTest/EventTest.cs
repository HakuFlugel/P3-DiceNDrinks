﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdministratorPanel;
using Shared;
using System.Windows.Forms;

namespace NUnitTest {
    [TestFixture]
    public class EventTest {
        FormProgressBar probar = new FormProgressBar();
        Event testEvent = new Event();
        EventsTab eventsTap;
        EventItem eventItem;

        public EventTest() {
            eventsTap = new EventsTab(probar);
            testEvent.id = 1;
            testEvent.name = "Weston Wensday";
            testEvent.description = "Something weston related on wensday";

            eventsTap.Evnts.Add(testEvent);
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
