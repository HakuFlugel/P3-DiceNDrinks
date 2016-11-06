using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Globalization;

namespace AdministratorPanel {
    class PriceElementPopupBox : FancyPopupBox {

        private ProductPopupBox productPopupBox;
        private PriceElement priceElement;

        NiceTextBox priceName = new NiceTextBox() {
            Width = 200,
            waterMark = "Price Name",
            Margin = new Padding(4, 10, 20, 10)
        };
        NiceTextBox price = new NiceTextBox() {
            Width = 200,
            waterMark = "Price Cost",
            Margin = new Padding(4, 10, 20, 10)
        };

        public PriceElementPopupBox(ProductPopupBox productPopupBox, PriceElement priceElement = null) {
            this.productPopupBox = productPopupBox;
            this.priceElement = priceElement;

            if (this.priceElement != null) {
                priceName.Text = priceElement.name;
                price.Text = priceElement.price.ToString();

            } else {
                Controls.Find("delete", true).First().Enabled = false;
            }
        }
        public PriceElementPopupBox() {

        }

        protected override Control CreateControls() {
            // outer panel
            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            // inner panels
            TableLayoutPanel rightPanel = new TableLayoutPanel();
            rightPanel.ColumnCount = 1;
            rightPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rightPanel.Dock = DockStyle.Fill;

            TableLayoutPanel leftPanel = new TableLayoutPanel();
            leftPanel.ColumnCount = 1;
            leftPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            leftPanel.Dock = DockStyle.Fill;

            rightPanel.Controls.Add(priceName);
            leftPanel.Controls.Add(price);

            header.Controls.Add(rightPanel);
            header.Controls.Add(leftPanel);
            return header;
        }

        protected override void delete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        protected override void save(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}
