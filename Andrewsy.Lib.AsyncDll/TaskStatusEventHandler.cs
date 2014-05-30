using System;
using System.Collections.Generic;
using System.Text;

namespace Andrewsy.Lib.AsyncDll
{
    /// <summary>
    /// 异步任务委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void TaskStatusEventHandler(object sender, TaskStatusArgs args);

}
