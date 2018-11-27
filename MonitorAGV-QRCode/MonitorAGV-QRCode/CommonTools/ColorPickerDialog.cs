using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ColorPickerDialog : Form
    {
        private ColorPickerCtrl m_colorPicker;

        private IContainer components = null;

        private TabControl m_tabControl;

        private TabPage m_knownColorsTabPage;

        private TabPage m_colorTabPage;

        private Button m_ok;

        private Button m_cancel;

        private ColorListBox m_colorList;

        public ColorPickerDialog()
        {
            this.InitializeComponent();
            this.m_colorPicker = new ColorPickerCtrl();
            this.m_colorPicker.Dock = DockStyle.Fill;
            this.m_colorTabPage.Controls.Add(this.m_colorPicker);
        }

        private void OnSelected(object sender, TabControlEventArgs e)
        {
            bool flag = e.TabPage == this.m_knownColorsTabPage;
            if (flag)
            {
                this.m_colorList.SelectColor(this.m_colorPicker.SelectedColor);
            }
            bool flag2 = e.TabPage == this.m_colorTabPage;
            if (flag2)
            {
                this.m_colorPicker.SelectedColor = (Color)this.m_colorList.SelectedItem;
            }
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.m_tabControl = new TabControl();
            this.m_colorTabPage = new TabPage();
            this.m_knownColorsTabPage = new TabPage();
            this.m_colorList = new ColorListBox();
            this.m_cancel = new Button();
            this.m_ok = new Button();
            this.m_tabControl.SuspendLayout();
            this.m_knownColorsTabPage.SuspendLayout();
            base.SuspendLayout();
            this.m_tabControl.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            this.m_tabControl.Controls.Add(this.m_colorTabPage);
            this.m_tabControl.Controls.Add(this.m_knownColorsTabPage);
            this.m_tabControl.Location = new Point(4, 5);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new Size(527, 282);
            this.m_tabControl.TabIndex = 1;
            this.m_tabControl.Selected += new TabControlEventHandler(this.OnSelected);
            this.m_colorTabPage.Location = new Point(4, 22);
            this.m_colorTabPage.Name = "m_colorTabPage";
            this.m_colorTabPage.Padding = new Padding(3);
            this.m_colorTabPage.Size = new Size(519, 256);
            this.m_colorTabPage.TabIndex = 0;
            this.m_colorTabPage.Text = "Colors";
            this.m_colorTabPage.UseVisualStyleBackColor = true;
            this.m_knownColorsTabPage.Controls.Add(this.m_colorList);
            this.m_knownColorsTabPage.Location = new Point(4, 22);
            this.m_knownColorsTabPage.Name = "m_knownColorsTabPage";
            this.m_knownColorsTabPage.Padding = new Padding(3);
            this.m_knownColorsTabPage.Size = new Size(519, 256);
            this.m_knownColorsTabPage.TabIndex = 1;
            this.m_knownColorsTabPage.Text = "Known Colors";
            this.m_knownColorsTabPage.UseVisualStyleBackColor = true;
            this.m_colorList.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.m_colorList.ColumnWidth = 170;
            this.m_colorList.DrawMode = DrawMode.OwnerDrawFixed;
            this.m_colorList.FormattingEnabled = true;
            this.m_colorList.ItemHeight = 26;
            this.m_colorList.Location = new Point(6, 10);
            this.m_colorList.MultiColumn = true;
            this.m_colorList.Name = "m_colorList";
            this.m_colorList.Size = new Size(506, 238);
            this.m_colorList.TabIndex = 0;
            this.m_cancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_cancel.DialogResult = DialogResult.Cancel;
            this.m_cancel.Location = new Point(452, 293);
            this.m_cancel.Name = "m_cancel";
            this.m_cancel.Size = new Size(75, 23);
            this.m_cancel.TabIndex = 2;
            this.m_cancel.Text = "&Cancel";
            this.m_cancel.UseVisualStyleBackColor = true;
            this.m_ok.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_ok.DialogResult = DialogResult.OK;
            this.m_ok.Location = new Point(371, 293);
            this.m_ok.Name = "m_ok";
            this.m_ok.Size = new Size(75, 23);
            this.m_ok.TabIndex = 2;
            this.m_ok.Text = "&OK";
            this.m_ok.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(534, 326);
            base.Controls.Add(this.m_ok);
            base.Controls.Add(this.m_cancel);
            base.Controls.Add(this.m_tabControl);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ColorPickerDialog";
            this.Text = "Color Picker";
            this.m_tabControl.ResumeLayout(false);
            this.m_knownColorsTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
