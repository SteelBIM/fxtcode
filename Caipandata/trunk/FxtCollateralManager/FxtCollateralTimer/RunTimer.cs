using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace FxtCollateralTimer
{
    public class RunTimer
    {
        /// <summary>
        /// 创建一个定时器对象。
        /// </summary>
        /// <param name="定时">指示定时时间，以毫秒为单位。</param>
        /// <param name="间歇时间">指示定时之中的间歇时间，用于检查是否取消执行。</param>
        public RunTimer(int 定时, int 间歇时间)
        {
            this.定时 = 定时;
            if (间歇时间 < 10) throw new Exception("间歇时间不得小于10毫秒!");
            this.间歇时间 = 间歇时间;
        }
        /// <summary>
        /// 指示定时时间，以毫秒为单位。
        /// </summary>
        public int 定时
        {
            get
            {
                return _定时;
            }
            set
            {
                _定时 = value;
            }
        }
        private int _定时;
        /// <summary>
        /// 指示定时之中的间歇时间，用于检查是否取消执行。
        /// </summary>
        public int 间歇时间
        {
            get
            {
                return _间歇时间;
            }
            set
            {
                _间歇时间 = value;
            }
        }
        private int _间歇时间;
        private BackgroundWorker 后台处理进程
        {
            get
            {
                return _后台处理进程;
            }
            set
            {
                _后台处理进程 = value;
            }
        }
        private BackgroundWorker _后台处理进程;
        private object 附件
        {
            get
            {
                return _附件;
            }
            set
            {
                _附件 = value;
            }
        }
        private object _附件;
        /// <summary>
        /// 指示定时器是否处于运行状态
        /// </summary>
        public bool 执行中
        {
            get
            {
                return _执行中;
            }
        }
        private bool _执行中;
        /// <summary>
        /// 启动定时器，如果定时器已经启动，则引发异常。
        /// </summary>
        /// <param name="附件">在定时完成时可能被使用到的传递对象。</param>
        public void 执行(object 附件)
        {
            if (执行中) throw new Exception("定时器已启动!");
            _执行中 = true;
            this.附件 = 附件;
            后台处理进程 = new BackgroundWorker();
            后台处理进程.WorkerSupportsCancellation = true;
            后台处理进程.DoWork += new DoWorkEventHandler(b_DoWork);
            后台处理进程.RunWorkerCompleted += new RunWorkerCompletedEventHandler(b_RunWorkerCompleted);
            后台处理进程.RunWorkerAsync(this);
        }
        /// <summary>
        /// 请求中止执行，如果定时器尚未启动，则引发异常。
        /// </summary>
        public void 中止(bool 取消触发完毕事件)
        {
            if (!执行中) throw new Exception("定时器尚未启动!");
            this.取消触发完毕事件 = 取消触发完毕事件;
            后台处理进程.CancelAsync();
        }
        /// <summary>
        /// 达到定时事件代理
        /// </summary>
        public delegate void 执行完毕代理(RunTimer sender, object 附件, bool 是否为用户取消);
        private bool 取消触发完毕事件
        {
            get
            {
                return _取消触发完毕事件;
            }
            set
            {
                _取消触发完毕事件 = value;
            }
        }
        private bool _取消触发完毕事件;
        /// <summary>
        /// 达到定时事件
        /// </summary>
        public event 执行完毕代理 执行完毕事件;

        void b_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) throw e.Error;
            if (!取消触发完毕事件 && 执行完毕事件 != null)
            {
                var o = e.Result as RunTimer;
                执行完毕事件(o, o.附件, e.Cancelled);
            }
            _执行中 = false;
            后台处理进程.Dispose();
        }

        void b_DoWork(object sender, DoWorkEventArgs e)
        {
            var o = e.Argument as RunTimer;
            e.Result = o;
            int x = 0;
            while (true)
            {
                if (x >= o.定时 || (sender as BackgroundWorker).CancellationPending) break;
                Thread.Sleep(o.间歇时间);
                x += o.间歇时间;
            }
        }
    }
}
