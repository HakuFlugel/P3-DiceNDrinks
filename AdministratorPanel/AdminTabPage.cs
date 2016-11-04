using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace AdministratorPanel {
    abstract public class AdminTabPage : TabPage {
        abstract public void Save();

        abstract public void Load();
            //using (StreamWriter sw = new StreamWriter("config.xml")) {
                //XmlSerializer ser = new XmlSerializer(typeof(MyFormState));
                //ser.Serialize(sw, state);
    }
}
