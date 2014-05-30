using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Andrewsy.Lib.AsyncDll
{
    /// <summary>
    /// 任务状态参数
    /// </summary>
    public class TaskStatusArgs
    {
        public TaskStatusArgs()
        { }

        public TaskStatusArgs(string title,string statusMsg)
        {
            _title = title;
            _statusMsg = statusMsg;
        }

        private string _title;
        /// <summary>
        /// 任务标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _statusMsg;
        /// <summary>
        /// 当前进度状态信息
        /// </summary>
        public string StatusMsg
        {
            get { return _statusMsg; }
            set { _statusMsg = value; }
        }

        private object _tag;
        /// <summary>
        /// 用户自定义参数
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private string _errorMsg;
        /// <summary>
        /// 异常错误信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            internal set { _errorMsg = value; }
        }

        private Thread _taskThread;
        /// <summary>
        /// 处理任务的线程
        /// </summary>
        internal Thread TaskThread
        {
            set { _taskThread = value; }
        }

        private int _threadID;
        /// <summary>
        /// 处理任务的线程ID
        /// </summary>
        public int ThreadID
        {
            get
            {
                if (_taskThread != null)
                {
                    _threadID = _taskThread.ManagedThreadId;       
                }
                return _threadID;
            }
        }

        private TaskStatus _taskStatus;
        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus TaskStatus
        {
            get { return _taskStatus; }
            internal set
            {
                _taskStatus = value;
                if (_taskThread != null)
                {
                    //取消任务，则任务线程终止
                    if (_taskStatus == AsyncDll.TaskStatus.Canceled)
                    {
                        try
                        {
                            _taskThread.Abort();
                        }
                        catch (ThreadAbortException ex)
                        {
                            _errorMsg = ex.Message;
                        }
                    }
                }
            }
        }

        private bool _canceled = false;
        /// <summary>
        /// 是否取消任务
        /// </summary>
        public bool Canceled
        {
            get { return _canceled; }
            set
            {
                _canceled = value;
                if (value)
                {
                    TaskStatus = AsyncDll.TaskStatus.Canceled;
                }
            }
        }
    }
}
