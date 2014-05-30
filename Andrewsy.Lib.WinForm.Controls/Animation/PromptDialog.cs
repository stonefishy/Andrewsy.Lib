using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Andrewsy.Lib.WinForm.Controls.Animation
{
    public partial class PromptDialog : AnimateForm
    {
        public event PromptEventHandler OnStarted =  null;//动画开始事件
        public event PromptEventHandler OnFinished = null;//动画结束事件

        private Orientation _orientation = Orientation.Vertical;//垂直方向
        private AnimationMode _animateType = AnimationMode.ShowAnimation;
        private Point _pLocal;

        private const int _timerInterval = 10;//默认为10
        private int _slideTime = 500;//默认500毫秒
        private int _horizontalStep = 0;//水平移动距离间隔
        private int _verticalStep = 0;//垂直方向移动距离间隔

        public PromptDialog()
            :base()
        {
            InitializeComponent();        
            InitFunction();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitFunction()
        {
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            InitArgs();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void InitArgs()
        {
            this._timer.Interval = _timerInterval;
            this.Width = 200;
            this.Height = 150;
            this._horizontalStep = this.Width / (_slideTime / _timerInterval);
            this._verticalStep = this.Height / (_slideTime / _timerInterval);
        }

        /// <summary>
        /// 滑动时间（毫秒）
        /// </summary>
        public int SlideTime
        {
            get { return _slideTime; }
            set
            {
                if (value > 1000)
                    _slideTime = 1000;
                else
                    _slideTime = value;

                InitArgs();
            }
        }

        /// <summary>
        /// 滑动方向方向
        /// </summary>
        public Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        /// <summary>
        /// 滑动窗体显示
        /// </summary>
        public void Show(Control parent)
        {
            InitShow(parent);
            base.Show();
        }

        /// <summary>
        /// 滑动窗体模式显示
        /// </summary>
        public void ShowDialog(Control parent)
        {
            InitShow(parent);
            this.TopMost = true;
            base.Show();
        }

        public new void Show()
        {
            throw new Exception("请用Show(Control parent)方法显示对象");
        }

        public new void ShowDialog()
        {
            throw new Exception("请用ShowDialog(Control parent)方法显示对象");
        }

        /// <summary>
        /// 初始显示
        /// </summary>
        private void InitShow(Control ctrl)
        {
            _animateType = AnimationMode.ShowAnimation;
            _pLocal = new Point(ctrl.Location.X + ctrl.Width, ctrl.Location.Y + ctrl.Height);

            if (_orientation == Orientation.Horizontal)
            {
                this.Location = new Point(_pLocal.X, this.Location.Y - this.Height);
            }
            else if (_orientation == Orientation.Vertical)
            {
                this.Location = new Point(_pLocal.X - this.Width, this._pLocal.Y);
            }
            this._timer.Enabled = true;

            if (OnStarted != null)
                OnStarted(this, new PromptEventArgs() { Orientation = _orientation });
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public new void Close()
        {
            _animateType = AnimationMode.CloseAnimation;
            this._timer.Enabled = true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public new void Hide()
        {
            _animateType = AnimationMode.HideAnimation;
            this._timer.Enabled = true;
        }

        /// <summary>
        /// 动画逻辑执行体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnAnimating(object sender, EventArgs e)
        {
            switch (_orientation)
            {
                case Orientation.Horizontal://水平方向
                    {
                        if (_animateType == AnimationMode.ShowAnimation)
                            this.Location = new Point(this.Location.X - _horizontalStep, this.Location.Y);
                        else
                            this.Location = new Point(this.Location.X + _horizontalStep, this.Location.Y);
                        break;
                    }
                case Orientation.Vertical://垂直方向
                    {
                        if (_animateType == AnimationMode.ShowAnimation)
                            this.Location = new Point(this.Location.X, this.Location.Y - _verticalStep);
                        else
                            this.Location = new Point(this.Location.X, this.Location.Y + _verticalStep);
                        break;
                    }
            }

            if (((this.Location.X+this.Width <= _pLocal.X) && (this.Orientation == Orientation.Horizontal)) || 
                ((this.Location.Y+this.Height <=_pLocal.Y) && (this.Orientation== Orientation.Vertical))||
                this.Location.Y > _pLocal.Y || this.Location.X > _pLocal.X)
            {
                this._timer.Enabled = false;

                if (_animateType == AnimationMode.HideAnimation)
                    base.Hide();
                else if (_animateType == AnimationMode.CloseAnimation)
                    base.Close();

                if (OnFinished != null)
                    OnFinished(this, new PromptEventArgs() { Orientation = _orientation });
            }

        }

        private void PromptDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Close();
                e.Cancel = true;
            }
        }
    }

    public enum Orientation
    {
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal,
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical
    }
}
