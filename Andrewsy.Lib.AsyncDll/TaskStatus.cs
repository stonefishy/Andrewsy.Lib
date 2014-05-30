using System;
using System.Collections.Generic;
using System.Text;

namespace Andrewsy.Lib.AsyncDll
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        UnStarted,
        /// <summary>
        /// 已开始
        /// </summary>
        Started,
        /// <summary>
        /// 正在处理中
        /// </summary>
        Processing,
        /// <summary>
        /// 已暂停、停止
        /// </summary>
        Stoped,
        /// <summary>
        /// 任务从暂停中唤醒，继续
        /// </summary>
        Resume,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled,
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed,
        /// <summary>
        /// 未知状态
        /// </summary>
        UnKnown
    }
}
