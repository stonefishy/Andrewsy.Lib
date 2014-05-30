using System;
using System.Collections.Generic;
using System.Text;

namespace Andrewsy.Lib.WinForm.Controls.Animation
{
    /// <summary>
    /// 淡入浅出参数
    /// </summary>
    public class FadeEventArgs:EventArgs
    {
        public FadeEventArgs()
            : base()
        {
        }

        public object Tag { get; set; }

        public FadeMode FadeType { get; set; }
    }
}
