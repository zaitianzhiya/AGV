using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class TreeIterator
    {
        public delegate TreeNode NodeCallback(TreeNode node, ref bool doContinue);

        protected TreeView m_tree;

        protected TreeIterator.NodeCallback m_callback;

        public TreeIterator(TreeView tree, TreeIterator.NodeCallback cb)
        {
            this.m_tree = tree;
            this.m_callback = cb;
        }

        public TreeNode Execute()
        {
            bool flag = false;
            TreeNode result;
            foreach (TreeNode node in this.m_tree.Nodes)
            {
                TreeNode treeNode = this.ExecuteNode(node, ref flag);
                bool flag2 = !flag;
                if (flag2)
                {
                    result = treeNode;
                    return result;
                }
            }
            result = null;
            return result;
        }

        protected TreeNode ExecuteNode(TreeNode node, ref bool doContinue)
        {
            doContinue = true;
            TreeNode treeNode = this.m_callback(node, ref doContinue);
            bool flag = !doContinue;
            TreeNode result;
            if (flag)
            {
                result = treeNode;
            }
            else
            {
                foreach (TreeNode node2 in node.Nodes)
                {
                    treeNode = this.ExecuteNode(node2, ref doContinue);
                    bool flag2 = !doContinue;
                    if (flag2)
                    {
                        result = treeNode;
                        return result;
                    }
                }
                result = null;
            }
            return result;
        }
    }
}
