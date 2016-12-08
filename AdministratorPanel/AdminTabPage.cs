using System.Windows.Forms;

namespace AdministratorPanel {
    abstract public class AdminTabPage : TabPage {
        abstract public void Save();

        abstract public void Load();
    }
}
