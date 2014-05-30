using System;
using System.Collections.Generic;
using System.Text;

namespace Andrewsy.Lib.AsyncDll
{
    public abstract class TaskAsyncAbstract:ITaskAsync
    {
        /// <summary>
        /// 开始任务
        /// </summary>
        public abstract void StartTask();

        /// <summary>
        /// 开始任务(带参数)
        /// </summary>
        /// <param name="tag">传入参数</param>
        public abstract void StartTask(object tag);

        /// <summary>
        /// 暂停任务
        /// </summary>
        //public abstract void StopTask();

        /// <summary>
        /// 将任务从暂停唤醒
        /// </summary>
        //public abstract void ResumeTask();

        /// <summary>
        /// 终止任务
        /// </summary>
        public abstract void AbortTask();

        /// <summary>
        /// 发送消息更换状态信息
        /// </summary>
        /// <param name="statusMsg">消息内容</param>
        public abstract void SendUpdateMessage(string statusMsg);

        /// <summary>
        /// 发送消息更换状态信息
        /// </summary>
        /// <param name="tag">对象消息</param>
        public abstract void SendUpdateMessage(object tag);

        /// <summary>
        /// 发送消息信息更换状态信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="statusMsg">消息内容</param>
        public abstract void SendUpdateMessage(string title, string statusMsg);

        /// <summary>
        /// 发送消息信息更换状态信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="statusMsg">消息内容</param>
        /// <param name="tag">自定义对象</param>
        public abstract void SendUpdateMessage(string title, string statusMsg, object tag);

    }
}
