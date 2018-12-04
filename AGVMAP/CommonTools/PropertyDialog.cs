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
    public class PropertyDialog : Form
    {
        private Size m_viewSize = Size.Empty;

        private Dictionary<object, object> m_dataObjects = new Dictionary<object, object>();

        private IContainer components = null;

        private Panel m_mainPanel;

        private Label m_label;

        private Panel m_rightPanel;

        private Button m_cancel;

        private Button m_ok;

        private ViewMap m_viewPanel;

        private Line line1;

        protected TreeView m_treeView;

        public PropertyDialog()
        {
            this.DoubleBuffered = true;
            this.InitializeComponent();
        }

        public void BeforeLoadPages()
        {
            this.m_viewSize = this.m_viewPanel.Size;
            this.m_treeView.BeginUpdate();
        }

        public TreeNode AddPage(object key, Control page, object dataobject)
        {
            return this.AddPage(key, page, null, dataobject);
        }

        public TreeNode AddPage(object key, Control page, TreeNode parentnode, object dataobject)
        {
            bool flag = page.Width > this.m_viewSize.Width;
            if (flag)
            {
                this.m_viewSize.Width = page.Width;
            }
            bool flag2 = page.Height > this.m_viewSize.Height;
            if (flag2)
            {
                this.m_viewSize.Height = page.Height;
            }
            this.m_viewPanel.AddView(key, page);
            this.m_dataObjects[key] = dataobject;
            TreeNode treeNode = new TreeNode(page.Text);
            treeNode.Tag = key;
            bool flag3 = parentnode != null;
            if (flag3)
            {
                parentnode.Nodes.Add(treeNode);
            }
            else
            {
                this.m_treeView.Nodes.Add(treeNode);
            }
            return treeNode;
        }

        public void AfterLoadPages()
        {
            bool flag = this.m_treeView.Nodes.Count > 0;
            if (flag)
            {
                this.m_treeView.SelectedNode = this.m_treeView.Nodes[0];
            }
            this.m_treeView.EndUpdate();
            int num = this.m_viewSize.Width - this.m_viewPanel.Width;
            int num2 = this.m_viewSize.Height - this.m_viewPanel.Height;
            bool flag2 = num < 0;
            if (flag2)
            {
                num = 0;
            }
            bool flag3 = num2 < 0;
            if (flag3)
            {
                num2 = 0;
            }
            base.Width += num;
            base.Height += num2;
            this.MinimumSize = base.Size;
        }

        public void SelectPage(object key)
        {
            TreeIteratorMatchTag treeIteratorMatchTag = new TreeIteratorMatchTag(this.m_treeView, key);
            TreeNode treeNode = treeIteratorMatchTag.Execute();
            bool flag = treeNode != null;
            if (flag)
            {
                this.m_treeView.SelectedNode = treeNode;
            }
        }

        private void OnAfterTreeSelect(object sender, TreeViewEventArgs e)
        {
            IPropertyDialogPage propertyDialogPage = this.m_viewPanel.GetView(this.m_viewPanel.CurKey) as IPropertyDialogPage;
            bool flag = propertyDialogPage != null;
            if (flag)
            {
                propertyDialogPage.BeforeDeactivated(this.m_dataObjects[this.m_viewPanel.CurKey]);
            }
            propertyDialogPage = (this.m_viewPanel.GetView(this.m_treeView.SelectedNode.Tag) as IPropertyDialogPage);
            bool flag2 = propertyDialogPage != null;
            if (flag2)
            {
                propertyDialogPage.BeforeActivated(this.m_dataObjects[this.m_treeView.SelectedNode.Tag]);
            }
            this.m_viewPanel.SelectView(this.m_treeView.SelectedNode.Tag);
            this.m_label.Text = this.m_treeView.SelectedNode.Text;
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
            this.m_mainPanel = new Panel();
            this.m_rightPanel = new Panel();
            this.m_label = new Label();
            this.m_treeView = new TreeView();
            this.m_cancel = new Button();
            this.m_ok = new Button();
            this.m_viewPanel = new ViewMap();
            this.line1 = new Line();
            this.m_mainPanel.SuspendLayout();
            this.m_rightPanel.SuspendLayout();
            base.SuspendLayout();
            this.m_mainPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.m_mainPanel.Controls.Add(this.m_rightPanel);
            this.m_mainPanel.Controls.Add(this.m_treeView);
            this.m_mainPanel.Location = new Point(4, 5);
            this.m_mainPanel.Name = "m_mainPanel";
            this.m_mainPanel.Size = new Size(445, 186);
            this.m_mainPanel.TabIndex = 0;
            this.m_rightPanel.Controls.Add(this.m_label);
            this.m_rightPanel.Controls.Add(this.m_viewPanel);
            this.m_rightPanel.Dock = DockStyle.Fill;
            this.m_rightPanel.Location = new Point(178, 0);
            this.m_rightPanel.Name = "m_rightPanel";
            this.m_rightPanel.Size = new Size(267, 186);
            this.m_rightPanel.TabIndex = 2;
            this.m_label.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            this.m_label.BackColor = SystemColors.ControlDark;
            this.m_label.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.m_label.ForeColor = SystemColors.Control;
            this.m_label.Location = new Point(4, 0);
            this.m_label.Margin = new Padding(3);
            this.m_label.Name = "m_label";
            this.m_label.Size = new Size(263, 20);
            this.m_label.TabIndex = 1;
            this.m_label.Text = "label1";
            this.m_label.TextAlign = ContentAlignment.MiddleLeft;
            this.m_treeView.Dock = DockStyle.Left;
            this.m_treeView.FullRowSelect = true;
            this.m_treeView.Location = new Point(0, 0);
            this.m_treeView.Name = "m_treeView";
            this.m_treeView.Size = new Size(178, 186);
            this.m_treeView.TabIndex = 0;
            this.m_treeView.AfterSelect += new TreeViewEventHandler(this.OnAfterTreeSelect);
            this.m_cancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_cancel.DialogResult = DialogResult.Cancel;
            this.m_cancel.Location = new Point(374, 202);
            this.m_cancel.Name = "m_cancel";
            this.m_cancel.Size = new Size(75, 23);
            this.m_cancel.TabIndex = 1;
            this.m_cancel.Text = "Cancel";
            this.m_cancel.UseVisualStyleBackColor = true;
            this.m_ok.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_ok.DialogResult = DialogResult.OK;
            this.m_ok.Location = new Point(293, 202);
            this.m_ok.Name = "m_ok";
            this.m_ok.Size = new Size(75, 23);
            this.m_ok.TabIndex = 1;
            this.m_ok.Text = "OK";
            this.m_ok.UseVisualStyleBackColor = true;
            this.m_viewPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.m_viewPanel.CurKey = null;
            this.m_viewPanel.Location = new Point(4, 26);
            this.m_viewPanel.Name = "m_viewPanel";
            this.m_viewPanel.Size = new Size(263, 160);
            this.m_viewPanel.TabIndex = 2;
            this.line1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.line1.ForeColor = SystemColors.ControlLight;
            this.line1.LinePositions = AnchorStyles.Top;
            this.line1.Location = new Point(4, 196);
            this.line1.Name = "line1";
            this.line1.Size = new Size(446, 10);
            this.line1.TabIndex = 2;
            this.line1.TabStop = false;
            this.line1.Text = "line1";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(454, 233);
            base.Controls.Add(this.m_ok);
            base.Controls.Add(this.m_cancel);
            base.Controls.Add(this.m_mainPanel);
            base.Controls.Add(this.line1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PropertyDialog";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Show;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "PropertyDialog";
            this.m_mainPanel.ResumeLayout(false);
            this.m_rightPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
