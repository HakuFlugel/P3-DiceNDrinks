﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Drawing;

namespace AdministratorPanel {
    class ProductItem : TableLayoutPanel {

        private int sizeX = 192;
        private int sizeY = 192;
        private Image image;

        public ProductItem(Product product) {

            Update(product);

            BackColor = Color.LightGray;
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize; // note skal måske ændres
            RowCount = 3;
            ColumnCount = 1;
            
            //1
            Panel pn = new Panel();
            pn.Height = 64;
            pn.Width = ClientSize.Width;
            
            pn.BackgroundImage = image;
            pn.BackgroundImageLayout = ImageLayout.Zoom;
            pn.BackColor = Color.Black;
            Controls.Add(pn);

            //2
            Label productName = new Label();
            productName.Dock = DockStyle.Fill;
            productName.Text = product.name;
            Controls.Add(productName);

            //3
            TableLayoutPanel priceTable = new TableLayoutPanel();
            priceTable.AutoSize = true;
            priceTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            priceTable.Dock = DockStyle.Fill;
            priceTable.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            priceTable.ColumnCount = 2;

            Controls.Add(Information(product,priceTable));

            this.MakeSuperClickable((sender, ev) => {Console.WriteLine("Loli-pop"); });
        }

        private TableLayoutPanel Information(Product p, TableLayoutPanel tb) {
            
            List<Label> lList = new List<Label>();
            
            foreach (var item in p.PriceElements) {
                StringBuilder prefix = new StringBuilder();
                StringBuilder price = new StringBuilder();

                Label preLabel = new Label();
                Label priceLabel = new Label();

                prefix.Append(item.name);
                Console.WriteLine(item.name);
                preLabel.Text = prefix.ToString();
                lList.Add(preLabel);
                
                price.Append("Price: ");
                price.Append(item.price);
                priceLabel.Text = price.ToString();
                lList.Add(priceLabel);
            }

            tb.Controls.AddRange(lList.ToArray());
            return tb;
        }

        private void Update(Product product){
            
            image = Image.FromFile(product.image);
           
            this.Height = sizeY;
            this.Width = sizeX;
            
        }
    }
}
