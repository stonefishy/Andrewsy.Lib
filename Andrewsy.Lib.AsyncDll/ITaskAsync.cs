using System;
using System.Collections.Generic;
using System.Text;

namespace Andrewsy.Lib.AsyncDll
{
    /// <summary>
    /// 异步任务接口,暂停/继续任务跟任务的细粒度有关,此处作为任务的通用性,取消暂停/继续功能
    /// </summary>
    public interface ITaskAsync
    {
        /// <summary>
        /// 开始任务
        /// </summary>
        void StartTask();

        /// <summary>
        /// 开始任务(带参数)
        /// </summary>
        /// <param name="tag"></param>
        void StartTask(object tag);


        /// <summary>
        /// 暂停任务
        /// </summary>
        //void StopTask();

        /// <summary>
        /// 继续任务
        /// </summary>
        //void ResumeTask();

        /// <summary>
        /// 终止任务
        /// </summary>
        void AbortTask();


    }
}
