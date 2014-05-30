using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Andrewsy.Lib.WinForm.Controls.Animation
{
    public partial class FadeForm : AnimateForm
    {
        public event FadeEventHandler OnStarted = null;//窗体显示之前触发事件
        public event FadeEventHandler OnFinished = null;//窗体关闭之后触发事件
        public event FadeEventHandler OnFormShowed = null;//窗体显示之后

        public FadeForm()
        {
            InitializeComponent();
            this._timer.Enabled = true;
        }

        /// <summary>
        /// 显示/关闭模式
        /// </summary>
        private FadeMode _fadeType = FadeMode.FadeIn;
        public FadeMode FadeType
        {
            get { return _fadeType; }
        }

        /// <summary>
        /// 浅入淡出逻辑控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnAnimating(object sender, EventArgs e)
        {
            if (_fadeType == FadeMode.FadeIn)
                this.Opacity += 0.1;
            else if (_fadeType == FadeMode.FadeOut)
                this.Opacity -= 0.1;

            if (this.Opacity >= 1 || this.Opacity<= 0)
            {
                this._timer.Enabled = false;

                if (_fadeType == FadeMode.FadeIn)
                {
                    if (OnFormShowed != null)
                        OnFormShowed(this, new FadeEventArgs() { FadeType = _fadeType });
                }
                //关闭
                if (_fadeType == FadeMode.FadeOut)
                {
                    if (_hideOrClose) base.Hide();
                    else base.Close();

                    if (OnFinished != null)
                        OnFinished(this, new FadeEventArgs() { FadeType = _fadeType });
                }
            }
        }

        /// <summary>
        /// 对话框显示
        /// </summary>
        public new void Show()
        {
            InitShow();
            base.Show();
        }

        /// <summary>
        /// 对话框模式显示
        /// </summary>
        public new void ShowDialog()
        {
            InitShow();
            base.ShowDialog();
        }

        /// <summary>
        /// 初始显示
        /// </summary>
        private void InitShow()
        {
            this._fadeType = FadeMode.FadeIn;
            this.Opacity = 0;
            this._timer.Enabled = true;

            if (OnStarted != null)
                OnStarted(this, new FadeEventArgs() { FadeType = _fadeType });
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public new  void Close()
        {
            this._fadeType = FadeMode.FadeOut;
            this._hideOrClose = false;
            this._timer.Enabled = true;
        }

        public new void Hide()
        {
            this._fadeType = FadeMode.FadeOut;
            this._hideOrClose = true;
            this._timer.Enabled = true;
        }

        public void HideNoAnimation()
        {
            base.Hide();
        }

        private void FadeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason == CloseReason.UserClosing)
            //{
            //    this.Close();
            //    e.Cancel = true;
            //}
        }
    }
}
