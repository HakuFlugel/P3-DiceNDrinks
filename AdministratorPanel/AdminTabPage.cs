using System.Windows.Forms;

namespace AdministratorPanel {
    public abstract class AdminTabPage : TabPage {
        public abstract void Save();

        public abstract void Load();

        public abstract void download();
    }
}
