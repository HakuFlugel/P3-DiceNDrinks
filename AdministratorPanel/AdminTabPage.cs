using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace AdministratorPanel {
    public class AdminTabPage : TabPage {
        public AdminTabPage() {
            
        }
        private void Save() {
            using (StreamWriter sw = new StreamWriter("config.xml")) {

                //XmlSerializer ser = new XmlSerializer(typeof(MyFormState));
                //ser.Serialize(sw, state);
            }
        }
    }
}
