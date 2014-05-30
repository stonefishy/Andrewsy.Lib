using System;
using System.Collections.Generic;
using System.Text;
using Andrewsy.Lib.WinForm.Controls.Animation;

namespace Andrewsy.Lib.WinForm.Controls
{
    public class PromptEventArgs
    {
        public PromptEventArgs() { }

        public Orientation Orientation { get; set; }

        public object Tag { get; set; }
    }
}
