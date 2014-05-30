using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.WinForm.Controls.Animation
{
    /// <summary>
    /// 动画显示窗体参数类
    /// </summary>
    public class SlideEventArgs:EventArgs
    {
        public SlideEventArgs() { }

        public SlideDirection SlideDirection { get; set; }

        public object Tag { get; set; }
    }
}
