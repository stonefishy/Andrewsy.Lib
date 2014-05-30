using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace Andrewsy.Lib.WinForm.Controls.ExtControl
{
    public class ListViewEx : ListView
    {
        private Color _rowBackColor1 = Color.White;                     //行1色
        private Color _rowBackColor2 = Color.FromArgb(253, 215, 248);   //行2色
        private Color _selectedColor = Color.FromArgb(75, 188, 254);    //选择行色

        public ListViewEx()
            : base()
        {
            base.OwnerDraw = true;
            base.ResizeRedraw = true;
            base.DoubleBuffered = true;
            base.View = View.Details;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        [Description("设置或获取行交替显示的颜色1")]
        [Category("自定义")]
        [DefaultValue(typeof(Color),"White")]
        public Color RowBackColor1
        {
            get { return _rowBackColor1; }
            set
            {
                _rowBackColor1 = value;
                base.Invalidate();
            }
        }

        [Description("设置或获取行交替显示的颜色2")]
        [Category("自定义")]
        [DefaultValue(typeof(Color),"253, 215, 248")]
        public Color RowBackColor2
        {
            get { return _rowBackColor2; }
            set
            {
                _rowBackColor2 = value;
                base.Invalidate();
            }
        }

        [Description("设置或获取选择行颜色")]
        [Category("自定义")]
        [DefaultValue(typeof(Color),"75,188,254")]
        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                base.Invalidate();
            }
        }

        [Description("设置或获取列表框中显示button空间的行列值")]
        [Category("自定义")]
        public int[] ControlColumns
        {
            get;
            set;
        }

        public new void Clear()
        {
            this.Items.Clear();
            this.Controls.Clear();
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (View != View.Details)
            {
                e.DrawDefault = true;
            }
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            base.OnDrawSubItem(e);

            //只支持Detail视图
            if (View != View.Details)
            {
                e.DrawDefault = true;
                return;
            }

            if (e.ItemIndex == -1)
            {
                return;
            }

            Rectangle bounds = e.Bounds;
            ListViewItemStates itemState = e.ItemState;
            Graphics g = e.Graphics;

            //绘制选择项
            if((itemState & ListViewItemStates.Selected) ==
                ListViewItemStates.Selected)
            {
                bounds.Height--;
                Color innerBorderColor = Color.FromArgb(200, 255, 255);
                RenderBackgroundInternal(g, bounds, _selectedColor, _selectedColor, innerBorderColor, 0.35f, true,
                    LinearGradientMode.Vertical);

                bounds.Height++;
            }
            else//交替绘制行
            {
                Color backColor = e.ItemIndex % 2 == 0 ?
                _rowBackColor1 : _rowBackColor2;

                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, bounds);
                }
            }

            TextFormatFlags flags = GetFormatFlags(e);
            
            //第一列
            if (e.ColumnIndex == 0)
            {
                if (this.CheckBoxes)
                {
                    Point pt = new Point(bounds.X + 2, bounds.Y + 2);
                    CheckBoxState state = e.Item.Checked ? CheckBoxState.CheckedNormal :
                        CheckBoxState.UncheckedNormal;

                    CheckBoxRenderer.DrawCheckBox(g, pt,state);
                }
                if (e.Item.ImageList == null)
                {
                    e.DrawText(flags);
                    return;
                }

                //绘制图标
                Image image = e.Item.ImageIndex == -1?
                    null:e.Item.ImageList.Images[e.Item.ImageIndex];

                if (image == null)
                {
                    e.DrawText(flags);
                    return;
                }

                Rectangle imageRect = bounds;

                //if (!CheckBoxes),CheckBox 和图片 无法同时显示
                    imageRect = new Rectangle(bounds.X + 4, bounds.Y + 2, bounds.Height - 2, bounds.Height - 2);
                //else
                //    imageRect = new Rectangle(bounds.X + 16, bounds.Y + 2, bounds.Height - 2, bounds.Height - 2);
                g.DrawImage(image, imageRect);

                Rectangle textRect = new Rectangle(imageRect.Right + 3, bounds.Y, bounds.Width - imageRect.Right - 3, bounds.Height);
                TextRenderer.DrawText(g, e.Item.Text, e.Item.Font, textRect, e.Item.ForeColor, flags);

                return;
            }

            e.DrawText(flags);

            //针对添加有控件的，绘制控件
            if (e.SubItem.Tag == null)
                return;
            Control ctrl = e.SubItem.Tag as Control;
            if (ctrl == null)
                return;

            this.Controls.Remove(ctrl);

            Rectangle rect = e.SubItem.Bounds;
            ctrl.Location = new Point(rect.X + 1, rect.Y + 1);
            ctrl.Size = new Size(rect.Width - 2, rect.Height - 2);

            this.Controls.Add(ctrl);
        }

        //获取文本显示格式
        private TextFormatFlags GetFormatFlags(DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Default;

            if (e.Header.TextAlign == HorizontalAlignment.Center)
            {
                flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
            }
            if (e.Header.Index == 0)
            {
                flags |= TextFormatFlags.VerticalCenter;
            }

            return flags | TextFormatFlags.EndEllipsis;
        }

        private void RenderBackgroundInternal(Graphics g,Rectangle bounds,Color backColor,Color borderColor,Color inerColor,float basePos,bool drawBorder,
            LinearGradientMode linearMode)
        {
            if (drawBorder)
            {
                bounds.Width--;
                bounds.Height--;
            }

            using(LinearGradientBrush brush = new LinearGradientBrush(
                bounds, Color.Transparent, Color.Transparent, linearMode))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(backColor, 0, 35, 24, 9);
                colors[1] = GetColor(backColor, 0, 13, 8, 3);
                colors[2] = backColor;
                colors[3] = GetColor(backColor, 0, 68, 69, 54);

                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePos, basePos + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, bounds);
            }

            if (backColor.A > 80)
            {
                Rectangle rectTop = bounds;
                if (linearMode == LinearGradientMode.Vertical)
                {
                    rectTop.Height = (int)(rectTop.Height * basePos);
                }
                else
                {
                    rectTop.Width = (int)(bounds.Width * basePos);
                }

                using (SolidBrush brushAlpha = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }
            if (drawBorder)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    g.DrawRectangle(pen, bounds);
                }

                bounds.Inflate(-1, -1);
                using (Pen pen = new Pen(inerColor))
                {
                    g.DrawRectangle(pen,bounds);
                }
            }
        }

        private static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a1 = colorBase.A;
            int r1 = colorBase.R;
            int g1 = colorBase.G;
            int b1 = colorBase.B;

            if (a + a1 > 255) a = 255; else a = Math.Max(a + a1, 0);
            if (r + r1 > 255) r = 255; else r = Math.Max(r + r1, 0);
            if (g + g1 > 255) g = 255; else g = Math.Max(g + g1, 0);
            if (b + b1 > 255) b = 255; else b = Math.Max(b + b1, 0);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
