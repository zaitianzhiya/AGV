using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class TreeIteratorMatchTag : TreeIterator
    {
        private object m_tagToMatch;

        public TreeIteratorMatchTag(TreeView tree, object tag)
            : base(tree, null)
        {
            this.m_tagToMatch = tag;
            this.m_callback = new TreeIterator.NodeCallback(this.nodeCallback);
        }

        private TreeNode nodeCallback(TreeNode node, ref bool doContinue)
        {
            bool flag = node.Tag != null && this.m_tagToMatch != null;
            TreeNode result;
            if (flag)
            {
                bool flag2 = node.Tag.Equals(this.m_tagToMatch);
                if (flag2)
                {
                    doContinue = false;
                    result = node;
                    return result;
                }
            }
            result = null;
            return result;
        }
    }
}
