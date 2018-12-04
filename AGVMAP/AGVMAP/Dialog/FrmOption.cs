using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGVMAP.HelpClass;
using Canvas.Layers;

namespace AGVMAP.Dialog
{
    public partial class FrmOption : BaseForm
    {
        public bool GridEnable;
        public GridLayer.eStyle GridStyle;
        public Color GridColor;
        public Color BackGroudColor;
        public Color PenColor;
        public float PenWidth;

        public FrmOption()
        {
            InitializeComponent();
        }

        public FrmOption(bool gridEnable,GridLayer.eStyle gridStyle, Color gridColor, Color backGroudColor, Color penColor, float penWidth)
        {
            InitializeComponent();
            this.GridEnable = gridEnable;
            this.GridStyle = gridStyle;
            this.GridColor = gridColor;
            this.BackGroudColor = backGroudColor;
            this.PenColor = penColor;
            this.PenWidth = penWidth;
        }

        private void FrmOption_Load(object sender, EventArgs e)
        {
            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = this.ForeColor;
            this.clorPk_bg.Color = this.BackGroudColor;
            this.clorPk_grid.Color = this.GridColor;
            this.ckeEnableGrid.Checked = this.GridEnable;
            this.rdoLines.Checked = (this.GridStyle == GridLayer.eStyle.Lines);
            this.rdoPoints.Checked = (this.GridStyle != GridLayer.eStyle.Lines);
            this.clorPk_pen.Color = this.PenColor;
            this.txtPenSize.Text = this.PenWidth.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.BackGroudColor = this.clorPk_bg.Color;
            this.GridEnable = this.ckeEnableGrid.Checked;
            this.GridColor = this.clorPk_grid.Color;
            this.GridStyle = (this.rdoLines.Checked ? GridLayer.eStyle.Lines : GridLayer.eStyle.Dots);
            this.PenColor = this.clorPk_pen.Color;
            this.PenWidth = (float)Convert.ToDouble(this.PenWidth);

            FileControl.SetFileControl.WriteIniValue("OPTION", "BgColorB", clorPk_bg.Color.A + "," + clorPk_bg.Color.R + "," + clorPk_bg.Color.G + "," + clorPk_bg.Color.B, Global.path);
            FileControl.SetFileControl.WriteIniValue("OPTION", "UseCoord", ckeEnableGrid.Checked.ToString(), Global.path);
            FileControl.SetFileControl.WriteIniValue("OPTION", "CoordType", GridStyle.ToString(), Global.path);
            FileControl.SetFileControl.WriteIniValue("OPTION", "CoordColor", clorPk_grid.Color.A + "," + clorPk_grid.Color.R + "," + clorPk_grid.Color.G + "," + clorPk_grid.Color.B, Global.path);
            FileControl.SetFileControl.WriteIniValue("OPTION", "PenColor", clorPk_pen.Color.A + "," + clorPk_pen.Color.R + "," + clorPk_pen.Color.G + "," + clorPk_pen.Color.B, Global.path);
            FileControl.SetFileControl.WriteIniValue("OPTION", "PenSize", txtPenSize.Text.Trim(), Global.path);

            base.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void FrmOption_Shown(object sender, EventArgs e)
        {
            clorPk_bg.Focus();
        }
    }
}
