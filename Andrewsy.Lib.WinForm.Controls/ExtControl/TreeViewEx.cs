using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Andrewsy.Lib.WinForm.Controls.ExtControl
{
    public partial class TreeViewEx : TreeView
    {

        private const int STATE_UNCHECKED = 0; //不选中状态
		private const int STATE_CHECKED = 1; //选中状态
		private const int STATE_MIXED = 2; //不确定状态

        public TreeViewEx():base()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }      

        public void UpdateView()
        {
            this.SetNodesState(this.Nodes);
        }

        public void SelectAll()
        {
            this.SetNodesState(this.Nodes,STATE_CHECKED);
        }

        public void UnselectAll()
        {
            this.SetNodesState(this.Nodes,STATE_UNCHECKED);
        }

        public void AntiSelect()
        {
            this.SetNodesToAntiState(this.Nodes);
        }

        //所有子节点是否选中
        public static bool IsAllChildrenChecked(TreeNode parent)
        {
            return IsAllChildrenSame(parent, STATE_CHECKED);
        }

        //所有子节点是否未选中
        public static bool IsAllChildrenUnchecked(TreeNode parent)
        {
            return IsAllChildrenSame(parent, STATE_UNCHECKED);
        }

        //设置节点的图像状态
        private void SetNodesState(TreeNodeCollection nodes,int state)
        {
            foreach (TreeNode node in nodes)
            {
                node.StateImageIndex = state;
                if (state == STATE_CHECKED)
                {
                    node.Checked = true;
                }
                else 
                {
                    node.Checked = false;    
                }

                if (node.Nodes.Count != 0)
                {
                    SetNodesState(node.Nodes, state);
                }
            }
        }

        private void SetNodesState(TreeNodeCollection nodes) {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    node.StateImageIndex = STATE_CHECKED;
                }
                else 
                {
                    node.StateImageIndex = STATE_UNCHECKED;
                }

                if (node.Nodes.Count != 0)
                {
                    SetNodesState(node.Nodes);
                }
            }
        }

        private void SetNodesToAntiState(TreeNodeCollection nodes) 
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    node.StateImageIndex = STATE_UNCHECKED;
                    node.Checked = false;
                }
                else
                {
                    node.StateImageIndex = STATE_CHECKED;
                    node.Checked = true;
                }

                if (node.Nodes.Count != 0)
                {
                    SetNodesToAntiState(node.Nodes);
                }
            }
        }        

        //用父结点的值更新子节点
        private void UpdateChildren(TreeNode parent)
        {
            int state = parent.StateImageIndex;
            foreach (TreeNode node in parent.Nodes)
            {
                node.StateImageIndex = state;

                switch (node.StateImageIndex)
                {
                    case STATE_UNCHECKED:
                        node.Checked = false;
                        break;
                    case STATE_CHECKED:
                        node.Checked = true;
                        break;
                }

                if (node.Nodes.Count != 0)
                {
                    UpdateChildren(node);
                }
            }
        }

        //根据子节点更新父结点
        private void UpdateParent(TreeNode child)
        {
            TreeNode parent = child.Parent;
            if (parent == null)
            {
                return;
            }

            if (child.StateImageIndex == STATE_MIXED)
            {
                parent.StateImageIndex = STATE_MIXED;
            }
            else if (IsAllChildrenChecked(parent))
            {
                parent.StateImageIndex = STATE_CHECKED;
            }
            else if (IsAllChildrenUnchecked(parent))
            {
                parent.StateImageIndex = STATE_UNCHECKED;
            }
            else
            {
                parent.StateImageIndex = STATE_MIXED;
            }
            UpdateParent(parent);
        }        

        //是否所有子节点具有统一状态
        private static bool IsAllChildrenSame(TreeNode parent, int state)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.StateImageIndex != state)
                {
                    return false;
                }
                if (node.Nodes.Count != 0 && !IsAllChildrenSame(node, state))
                {
                    return false;
                }
            }
            return true;
        }

       //获取选取图，未选取图，不确定图
        private static Image GetStateImage(CheckBoxState state, Size imageSize)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Point pt = new Point((16 - imageSize.Width) / 2, (16 - imageSize.Height) / 2);
                CheckBoxRenderer.DrawCheckBox(g, pt, state);
            }
            return bmp;
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.StateImageList = new ImageList();            
            using (Graphics g = base.CreateGraphics())
            {
                Size glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
                this.StateImageList.Images.Add(GetStateImage(CheckBoxState.UncheckedNormal, glyphSize));
                this.StateImageList.Images.Add(GetStateImage(CheckBoxState.CheckedNormal, glyphSize));
                this.StateImageList.Images.Add(GetStateImage(CheckBoxState.MixedNormal, glyphSize));
                this.StateImageList.Images.Add(GetStateImage(CheckBoxState.UncheckedDisabled, glyphSize));
                this.StateImageList.Images.Add(GetStateImage(CheckBoxState.CheckedDisabled, glyphSize));
                this.StateImageList.Images.Add(GetStateImage(CheckBoxState.MixedDisabled, glyphSize));
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {  
            base.OnPaint(e);
            UpdateView();
            
        }

        //用户点击节点重写
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                TreeViewHitTestInfo info = base.HitTest(e.Location);
                if (info.Node != null && info.Location == TreeViewHitTestLocations.StateImage)
                {
                    TreeNode node = info.Node;
                    switch (node.StateImageIndex)
                    {
                        case STATE_UNCHECKED:
                        case STATE_MIXED:
                            node.StateImageIndex = STATE_CHECKED;
                            node.Checked = true;
                            break;
                        case STATE_CHECKED:
                            node.StateImageIndex = STATE_UNCHECKED;
                            node.Checked = false;
                            break;
                    }
                    UpdateChildren(node);
                    UpdateParent(node);
                }
            }

            
        }

        //check if user press the space key
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                if (base.SelectedNode != null)
                {
                    TreeNode node = base.SelectedNode;
                    switch (node.StateImageIndex)
                    {
                        case STATE_UNCHECKED:
                        case STATE_MIXED:
                            node.StateImageIndex = STATE_CHECKED;
                            break;
                        case STATE_CHECKED:
                            node.StateImageIndex = STATE_UNCHECKED;
                            break;
                    }
                    UpdateChildren(node);
                    UpdateParent(node);
                }
            }
        }

       //用户Enter键选取
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            for (int i = 0; i < 3; i++)
            {
                Image img = this.StateImageList.Images[0];
                this.StateImageList.Images.RemoveAt(0);
                this.StateImageList.Images.Add(img);
            }
        }
	}
}

