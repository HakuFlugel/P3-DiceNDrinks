using System.Windows.Forms;

namespace AdministratorPanel {
    class ProductSectionItem : TableLayoutPanel{
        public FlowLayoutPanel headerFlowLayoutPanel = new FlowLayoutPanel();

        public ProductSectionItem(string section) {
            Name = section;
            Dock = DockStyle.Fill;
            AutoSize = true;

            ColumnCount = 1;
            RowCount = 2;

            //Title for the section
            Label title = new Label();
            title.Text = section;
            title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            title.Dock = DockStyle.Fill;
            title.Font = new System.Drawing.Font(title.Font.Name, title.Font.Size * 4);
            title.AutoSize = true;

            Controls.Add(title);

            // section content
            headerFlowLayoutPanel.Dock = DockStyle.Fill;
            headerFlowLayoutPanel.AutoSize = true;
            headerFlowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            Controls.Add(headerFlowLayoutPanel);
        }

        public void AddItem(Control ctr) {
            headerFlowLayoutPanel.Controls.Add(ctr);
        }
    }
}
