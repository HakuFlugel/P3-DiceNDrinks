using System.Collections.Generic;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    class ProductSectionItem : TableLayoutPanel{
        private FlowLayoutPanel flowPanel = new FlowLayoutPanel();


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
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoSize = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            Controls.Add(flowPanel);
        }

        public void AddItem(Control ctr) {
            flowPanel.Controls.Add(ctr);
        }
    }
}
