using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Threading;

namespace Andrewsy.Lib.AsyncDll
{
    public class TaskAsync:TaskAsyncAbstract
    {
        /// <summary>
        /// 需要处理的任务事件，即就是任务处理逻辑,可以订阅多个
        /// </summary>
        public event TaskStatusEventHandler OnTaskHandler = null;

        /// <summary>
        /// 当任务开始处理时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessStarted = null;

        /// <summary>
        /// 当任务正在处理时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessing = null;

        /// <summary>
        /// 当任务处理出现异常错误时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessError = null;

        /// <summary>
        /// 当任务处理取消时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessCanceled = null;

        /// <summary>
        /// 任务处理暂停时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessStoped = null;

        /// <summary>
        /// 当任务处理从暂停为唤醒时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessResumed = null;

        /// <summary>
        /// 当任务处理完成时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnProcessed = null;

        /// <summary>
        /// 当任务处理状态发生改变时，调用此事件
        /// </summary>
        public event TaskStatusEventHandler OnTaskStatusChanged = null;

        /// <summary>
        /// 异步任务委托
        /// </summary>
        /// <param name="args">参数</param>
        protected delegate void TaskAsyncHandler(TaskStatusArgs args);

        /// <summary>
        /// 更新界面控件的委托
        /// </summary>
        /// <param name="args"></param>
        protected delegate void UpdateUIHandler(TaskStatusArgs args);

        private TaskStatusArgs _statusArgs = null;  //状态参数信息
        private IAsyncResult _asyncResult = null;   //异步结果

        public TaskAsync()
        {
            _statusArgs = new TaskStatusArgs();
            _statusArgs.TaskStatus = TaskStatus.UnStarted;
        }

        private ContainerControl _updateControl;    //需要被更新的UI控件

        /// <summary>
        /// 带UI控件的构造函数，可实现异步处理任务的同时更新界面
        /// </summary>
        /// <param name="updateControl"></param>
        public TaskAsync(ContainerControl updateControl)
            :this()
        {
            _updateControl = updateControl;
        }

        
        /// <summary>
        /// 开始任务
        /// </summary>
        public override void StartTask()
        {
            if (_statusArgs.TaskStatus == TaskStatus.Started ||
                _statusArgs.TaskStatus == TaskStatus.Processing ||
                _statusArgs.TaskStatus == TaskStatus.Started ||
                _statusArgs.TaskStatus == TaskStatus.Stoped)
                return;

            TaskAsyncHandler taskHandler = new TaskAsyncHandler(TaskAsyncProcessed);
            _statusArgs.TaskStatus = TaskStatus.Started;
            taskHandler.BeginInvoke(_statusArgs, new AsyncCallback(TaskAsyncCallBack), _statusArgs);
        }

        /// <summary>
        /// 开始任务(带参数)
        /// </summary>
        /// <param name="tag"></param>
        public override void StartTask(object tag)
        {
            if (_statusArgs.TaskStatus == TaskStatus.Started ||
                _statusArgs.TaskStatus == TaskStatus.Processing ||
                _statusArgs.TaskStatus == TaskStatus.Started ||
                _statusArgs.TaskStatus == TaskStatus.Stoped)
                return;

            TaskAsyncHandler taskHandler = new TaskAsyncHandler(TaskAsyncProcessed);
            _statusArgs.TaskStatus = TaskStatus.Started;
            _statusArgs.Tag = tag;
            _asyncResult = taskHandler.BeginInvoke(_statusArgs, new AsyncCallback(TaskAsyncCallBack), _statusArgs);
        }

        /// <summary>
        /// 阻止当前线程，直到当前线程收到信息号
        /// </summary>
        public void WaitOne()
        {
            if (_asyncResult == null)
                return;

            if (!_asyncResult.IsCompleted)
            {
                Thread.Sleep(1000);
            }
            //_asyncResult.AsyncWaitHandle.WaitOne();
        }

        /// <summary>
        /// 异步任务处理回调
        /// </summary>
        /// <param name="ar"></param>
        private void TaskAsyncCallBack(IAsyncResult ar)
        {
            TaskStatusArgs args = ar.AsyncState as TaskStatusArgs;
            TaskAsyncHandler task = ((AsyncResult)ar).AsyncDelegate as TaskAsyncHandler;
            task.EndInvoke(ar);

            //更新任务进度信息
            UpdateProcessInfo(args);
        }

        /// <summary>
        /// 异步任务处理逻辑
        /// </summary>
        /// <param name="args"></param>
        private void TaskAsyncProcessed(TaskStatusArgs args)
        {
            try
            {
                args.TaskStatus = TaskStatus.Started;
                UpdateProcessInfo(args);

                //开始处理数据
                args.TaskStatus = TaskStatus.Processing;
                if (OnTaskHandler != null)
                {
                    OnTaskHandler(this, args);
                }
                args.TaskStatus = TaskStatus.Completed;
            }
            catch (ThreadAbortException ex)
            {
                //对任务线程终止,不做处理
            }
            catch (Exception ex)
            {
                //处理出错处理
                args.ErrorMsg = ex.Message;
                args.TaskStatus = TaskStatus.Failed;
            }
        }

        /// <summary>
        /// 更新处理信息
        /// </summary>
        /// <param name="args"></param>
        private void UpdateProcessInfo(TaskStatusArgs args)
        {
            try
            {
                //界面UI控件更新
                if (_updateControl != null)
                {
                    if (_updateControl.InvokeRequired)
                    {
                        _updateControl.Invoke(new UpdateUIHandler(TrigerEvent), new object[] { args });
                    }
                    else
                    {
                        TrigerEvent(args);
                    }
                }
                else
                {
                    TrigerEvent(args);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 触发状态事件事件
        /// </summary>
        /// <param name="args"></param>
        private void TrigerEvent(TaskStatusArgs args)
        {
            switch (args.TaskStatus)
            {
                case TaskStatus.Started:    //任务开始
                    if (OnProcessStarted != null) OnProcessStarted(this, args);
                    //if (OnProcessing != null) OnProcessing(this,args);
                    break;
                case TaskStatus.Processing: //任务处理中
                    if (OnProcessing != null) OnProcessing(this, args);
                    break;
                case TaskStatus.Stoped:     //任务暂停
                    if (OnProcessStoped != null) OnProcessStoped(this, args);
                    break;
                case TaskStatus.Resume:     //任务继续
                    if (OnProcessResumed != null) OnProcessResumed(this, args);
                    //if (OnProcessing != null) OnProcessing(this, args);
                    break;
                case TaskStatus.Canceled:   //任务取消
                    if (OnProcessCanceled != null) OnProcessCanceled(this, args);
                    //if(OnProcessed != null) OnProcessed(this,args);
                    break;
                case TaskStatus.Failed:     //任务错误
                    if (OnProcessError != null) OnProcessError(this, args);
                    //if (OnProcessed != null) OnProcessed(this, args);
                    break;
                case TaskStatus.Completed:  //任务完成
                    if (OnProcessed != null) OnProcessed(this, args);
                    break;
            }

            //任务状态改变
            if (OnTaskStatusChanged != null)
                OnTaskStatusChanged(this, args);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        //public override void StopTask()
        //{
               
        //}

        /// <summary>
        /// 将任务从暂停唤醒
        /// </summary>
        //public override void ResumeTask()
        //{

        //}

        /// <summary>
        /// 终止任务
        /// </summary>
        public override void AbortTask()
        {
            _statusArgs.Canceled = true;
            _statusArgs.TaskStatus = TaskStatus.Canceled;
            UpdateProcessInfo(_statusArgs);
        }

        /// <summary>
        /// 发送消息更换状态信息
        /// </summary>
        /// <param name="statusMsg">消息内容</param>
        public override void SendUpdateMessage(string statusMsg)
        {
            _statusArgs.StatusMsg = statusMsg;
            UpdateProcessInfo(_statusArgs);
        }

        /// <summary>
        /// 发送消息更换状态信息
        /// </summary>
        /// <param name="tag">对象消息</param>
        public override void SendUpdateMessage(object tag)
        {
            _statusArgs.Tag = tag;
            UpdateProcessInfo(_statusArgs);
        }

        /// <summary>
        /// 发送消息信息更换状态信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="statusMsg">消息内容</param>
        public override void SendUpdateMessage(string title, string statusMsg)
        {
            _statusArgs.Title = title;
            _statusArgs.StatusMsg = statusMsg;
            UpdateProcessInfo(_statusArgs);
        }

        /// <summary>
        /// 发送消息信息更换状态信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="statusMsg">消息内容</param>
        /// <param name="tag">自定义对象</param>
        public override void SendUpdateMessage(string title, string statusMsg, object tag)
        {
            _statusArgs.Title = title;
            _statusArgs.StatusMsg = statusMsg;
            _statusArgs.Tag = tag;
            UpdateProcessInfo(_statusArgs);
        }
    }
}
