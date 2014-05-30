using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Andrewsy.Lib.WinForm.Controls.Animation;

namespace Andrewsy.Lib.WinForm.Controls.Animation
{
    /// <summary>
    /// 动画窗体类
    /// </summary>
    public partial class SlideForm : AnimateForm
    {
        public event AnimationEventHandler OnStarted =  null;//动画开始事件
        public event AnimationEventHandler OnFinished = null;//动画结束事件

        private SlideDirection _slideDirection = SlideDirection.Right;//右方向动画
        private FadeMode _fadeType = FadeMode.FadeIn;

        private const int _timerInterval = 10;//默认为10
        private const int OffsetHorizontal = 100;//窗体水平偏移量
        private const int OffsetVertical = 100;//窗体垂直偏移量
        private int _slideTime = 500;//默认500毫秒
        private double _opacityStep = 0.00;//透明度间隔
        private int _horizontalStep = 0;//水平移动距离间隔
        private int _verticalStep = 0;//垂直方向移动距离间隔

        public SlideForm()
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
            this.StartPosition = FormStartPosition.CenterScreen;
            InitArgs();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void InitArgs()
        {
            this._timer.Interval = _timerInterval;
            this._horizontalStep = OffsetHorizontal / (_slideTime / _timerInterval);
            this._verticalStep = OffsetVertical / (_slideTime / _timerInterval);
            this._opacityStep = (double)(1.00 / (_slideTime / _timerInterval));
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
        public SlideDirection SlideDirection
        {
            get { return _slideDirection; }
            set { _slideDirection = value; }
        }

        /// <summary>
        /// 滑动窗体显示
        /// </summary>
        public new void Show()
        {
            InitShow();
            base.Show();
        }

        /// <summary>
        /// 滑动窗体模式显示
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

            if (_slideDirection == SlideDirection.Right)
                this.Location = new Point(this.Location.X - OffsetHorizontal, this.Location.Y);
            else if (_slideDirection == SlideDirection.Left)
                this.Location = new Point(this.Location.X + OffsetHorizontal, this.Location.Y);
            else if (_slideDirection == SlideDirection.Up)
                this.Location = new Point(this.Location.X, this.Location.Y + OffsetVertical);
            else if (_slideDirection == SlideDirection.Down)
                this.Location = new Point(this.Location.X, this.Location.Y - OffsetVertical);

            this._timer.Enabled = true;

            if (OnStarted != null)
                OnStarted(this, new SlideEventArgs() { SlideDirection = _slideDirection });
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public new void Close()
        {
            this._fadeType = FadeMode.FadeOut;
            this._timer.Enabled = true;
        }

        /// <summary>
        /// 动画逻辑执行体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnAnimating(object sender, EventArgs e)
        {
            //设置透明度
            if (this._fadeType == FadeMode.FadeIn)
                this.Opacity += _opacityStep;
            else if (this._fadeType == FadeMode.FadeOut)
                this.Opacity -= _opacityStep;
 
            switch (_slideDirection)
            {
                case Animation.SlideDirection.Left://向左方向滑动
                    this.Location = new Point(this.Location.X - _horizontalStep, this.Location.Y);
                    break;
                case Animation.SlideDirection.Right://向右方向滑动
                    this.Location = new Point(this.Location.X + _horizontalStep, this.Location.Y);
                    break;
                case Animation.SlideDirection.Up://向上方向滑动
                    this.Location = new Point(this.Location.X, this.Location.Y - _verticalStep);
                    break;
                case Animation.SlideDirection.Down://乡下方向滑动
                    this.Location = new Point(this.Location.X, this.Location.Y + _verticalStep);
                    break;
                default:                           //默认向右方向滑动
                    this.Location = new Point(this.Location.X + _horizontalStep, this.Location.Y);
                    break;
            }

            if (this.Opacity == 1 || this.Opacity == 0)
            {
                this._timer.Enabled = false;
                if (this._fadeType == FadeMode.FadeOut)
                {
                    base.Close();

                    if (OnFinished != null)
                        OnFinished(this, new SlideEventArgs() { SlideDirection = _slideDirection });
                }
            }

        }

        private void SlideForm_FormClosing(object sender, FormClosingEventArgs e)
        {    
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Close();
            }
        }

    }
}
