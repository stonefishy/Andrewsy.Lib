using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Andrewsy.Lib.WinForm.Controls.ExtControl
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ListViewSubItemEx
    {
        public static void AddControl(this ListViewItem.ListViewSubItem subItem, Control ctrl,ListView parent)
        {
            if (ctrl == null || parent == null)
                return;

            Rectangle rect = subItem.Bounds;
            ctrl.Location = new Point(rect.X + 1, rect.Y + 1);
            ctrl.Size = new Size(rect.Width - 2, rect.Height - 2);

            parent.Controls.Add(ctrl);
            subItem.Tag = ctrl;
        }
    }
}
