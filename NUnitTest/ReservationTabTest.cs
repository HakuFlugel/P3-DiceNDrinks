using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using AdministratorPanel;

namespace NUnitTest {
    [TestFixture]
    public class ReservationTabTest {
        ReservationTab resevationTest;
        FormProgressBar probar = new FormProgressBar();
        ReservationController resControl = new ReservationController();
        string name = "Bo Pedersen", email = "bo@karlsen.com", phone = "12345678";
        int numPeople = 3; DateTime time = DateTime.Now.AddDays(+3);

        Reservation testres = new Reservation() {
            created = DateTime.Today.AddDays(-1),
        };

        public ReservationTabTest() {
            resevationTest = new ReservationTab(resControl,probar);
            
            testres.name = name; testres.email = email; testres.phone = phone; testres.numPeople = numPeople; testres.time = time;
          
        }

        [Test]
        public void ReservationGetsAddedCorrectly() {
            resControl.addReservation(testres);
            Assert.True(resControl.findDay(time).reservations.Any(x => x.name == name && x.email == email && x.phone == phone && x.name == name && x.numPeople == numPeople));
        }
        
        [Test]
        public void CheckIfDayIsCreatedWhenLockingADay() {
            resevationTest.lockResevations.Checked = true;
            resevationTest.CheckedChanged();
            

            Assert.True(resControl.reservationsCalendar.Contains(resControl.findDay(DateTime.Today)));
        }

        
    }
}
