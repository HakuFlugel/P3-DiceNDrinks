﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
using System.ComponentModel;
using System.Media;

namespace AdministratorPanel
{
    /*abstract*/

    public abstract class FancyPopupBox : Form
    {
        protected Padding labelPadding = new Padding(5, 0, 5, 0);
        protected Padding otherPadding = new Padding(5, 0, 5, 20);
        public bool hasBeenChanged = false;

        public FancyPopupBox(bool canDelete = true)
        {
            MaximizeBox = false;
            //MinimumSize = new Size(800, 600);
            //MaximumSize = MinimumSize;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            TableLayoutPanel container = new TableLayoutPanel();
            container.RowCount = 2;
            container.AutoSize = true;
            container.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(container);


            container.Controls.Add(CreateControls());



            Panel p = new Panel();
            p.Height = 48;
            //p.AutoSize = true;
            p.Dock = DockStyle.Bottom;
            p.BackColor = Color.Transparent;
            p.Padding = new Padding(8);
            container.Controls.Add(p);

            if (canDelete)
            {
                Button delete = new Button();
                delete.Text = "Delete";
                delete.Name = "delete";
                delete.Dock = DockStyle.Left;
                delete.Click += this.delete;
                p.Controls.Add(delete);
            }

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.Name = "cancel";

            cancel.Dock = DockStyle.Right;
            cancel.Click += this.cancel;
            p.Controls.Add(cancel);

            Button save = new Button();
            save.Text = "Save";
            save.Name = "save";
            save.Dock = DockStyle.Right;
            save.Click += this.save;
            p.Controls.Add(save);


        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            switch (e.CloseReason) {
                case CloseReason.UserClosing:
                    SystemSounds.Question.Play();
                    //TODO: see product on close for messagebox text
                    if (hasBeenChanged && DialogResult.No ==
                        MessageBox.Show("Are you sure? Everything unsaved will be lost.",
                            "About to close", MessageBoxButtons.YesNo)) {

                        e.Cancel = true;
                    }

                    break;
            }
            base.OnFormClosing(e);
        }

        protected abstract Control CreateControls();

        protected virtual void save(object sender, EventArgs e) {
            
            hasBeenChanged = false;
            Close();
        }

        protected abstract void delete(object sender, EventArgs e);

        private void cancel(object sender, EventArgs e)
        {
            Close();
        }
    }

    class TestPopupbox : FancyPopupBox
    {
        //for products
        protected Product product;

        public TestPopupbox(Product product)
        {

            this.product = product;


        }

        protected override void delete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void save(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override Control CreateControls()
        {


            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            Controls.Add(tableLayoutPanel);

            for (int ppp = 0; ppp < 2; ppp++)
            {
                TableLayoutPanel tp = new TableLayoutPanel();
                tp.ColumnCount = 1;
                tp.Dock = DockStyle.Fill;
                tp.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
                tp.AutoSize = true;
                tableLayoutPanel.Controls.Add(tp);
                for (int i = 0; i < 20; i++)
                {
                    NiceTextBox tb = new NiceTextBox();
                    //tb.Dock = DockStyle.Fill;
                    tb.waterMark = i.ToString();
                    tb.clearable = true;
                    tb.Width = 400;

                    tp.Controls.Add(tb);
                }
            }

            return tableLayoutPanel;
        }

    }
}