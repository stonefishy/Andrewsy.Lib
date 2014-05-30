using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Andrewsy.Lib.WinForm.Controls.Animation
{
    public partial class AnimateForm : BaseForm
    {
        protected Timer _timer = null;//时间控制
        public AnimateForm()
        {
            InitializeComponent();
            this._timer = new Timer();
            this._timer.Tick += new EventHandler(_timer_Tick);
        }

        protected bool _hideOrClose = true;//隐藏窗体还是关闭窗体

        /// <summary>
        /// 动画逻辑实现，交由派生类实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAnimating(object sender, EventArgs e)
        {
            throw new NotImplementedException("没有实现动画逻辑体，请在子类中重写该功能!");
        }  

        /// <summary>
        /// 动画执行体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            OnAnimating(sender, e);
        }

        public new void ShowDialog()
        {
            base.ShowDialog();
        }
    }
}
