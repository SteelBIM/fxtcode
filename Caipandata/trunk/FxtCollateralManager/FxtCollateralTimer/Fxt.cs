using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using tTime = System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;
using System.Net;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtCommonLibrary.LibraryUtils;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using FxtCollateralManager.Common;
using BackgroundWorkerTaskTimer;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using System.Configuration;

namespace FxtCollateralTimer
{
    public partial class Fxt : Form
    {
        tTime.Thread thread;
        tTime.Timer threeadingTimer;
        int iReass = 0;
        string strIcon = @"ICO\crw.ico";
        List<int> listExists = new List<int>();
        List<TaskControl> listTaskControl = new List<TaskControl>();
        public Fxt()
        {
            InitializeComponent();
            this.Load += Fxt_Load;
            //butReass.Click += butReass_Click;
            this.Icon = new Icon(strIcon);
        }


        #region 任务栏图标
        void Fxt_Load(object sender, EventArgs e)
        {
            //设置窗体图标在任务通知栏中的相关属性
            this.notifyIcon1 = new NotifyIcon(this.components);
            this.notifyIcon1.Icon = new Icon(strIcon);
            this.notifyIcon1.Visible = false;
            this.notifyIcon1.Click += notifyIcon1_Click;
            this.SizeChanged += Fxt_SizeChanged;

            InitDataGridView(dgvTack);
            AddGridViewColumns(dgvTack);
            OpeaterThread();
        }

        void Fxt_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.notifyIcon1.Visible = true;
            }
        }
        //点击任务栏图标
        void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
        }
        #endregion

        #region 检查任务
        void butReass_Click(object sender, EventArgs e)
        {
            OpeaterThread();
        }
        ManualResetEvent opeaterManualRest = new ManualResetEvent(true);
        private void OpeaterThread()
        {
            if (iReass.Equals(1))
            {
                thread.Interrupt();
                thread.Abort();
                threeadingTimer.Dispose();
                iReass = 0;
                textMessage.AppendText("停止...\n");
                thread = null;
            }
            else
            {
                thread = new tTime.Thread(new tTime.ThreadStart(RunReassess));
                thread.IsBackground = true;
                textMessage.AppendText("请稍后...\n");
                thread.Start();
                iReass = 1;
            }
        }

        private void RunReassess()
        {
            int time = 10000 * 3; //定义30秒一次  
            tTime.TimerCallback timerDelagete = new tTime.TimerCallback(ReassessCallback);
            threeadingTimer = new tTime.Timer(timerDelagete, null, time, time);
        }

        void ReassessCallback(Object obj)
        {
            opeaterManualRest.WaitOne();
            if (textMessage.InvokeRequired)
            {
                BeginInvoke(new EventHandler(RunsOnWorkerThread), null);
            }
            else
            {
                RunsOnWorkerThread(this, null);
            }
        }
        int current = 0;
        private void RunsOnWorkerThread(object sender, EventArgs e)
        {
            opeaterManualRest.Reset();
            int count = listTaskControl.Count;
            current = 0;
            current = IsStop();
            if (listTaskControl.Count.Equals(0) || count == current)
            {
                opeaterManualRest.Set();
                AddBatch();
                textMessage.AppendText("进行中...\n");
            }
        }

        int IsStop()
        {
            if (dgvTack.Rows.Count > 0)
            {
                foreach (var task in listTaskControl)
                {
                    SystemTask systemTask = task.SystemTask;

                    string strDown = GetDownString(GetUrl(string.Format("API/FxtTask.svc/TaskById/{0}", systemTask.Id)));

                    if (Utils.GetJObjectValue(strDown, "Type").Equals("1"))//是否存在该任务
                    {
                        SysTask sysTask = Utils.Deserialize<SysTask>(Utils.GetJObjectValue(strDown, "Data"));
                        if (sysTask.Status.Equals(2))//任务是否需要暂停
                        {
                            task.BackgroundWorker.Wait();
                            systemTask.RowObject.Cells["RunStatus"].Value = "暂停";
                        }
                        else if (sysTask.Status.Equals(0))//任务是否需要继续
                        {
                            task.BackgroundWorker.Resume();
                            systemTask.RowObject.Cells["RunStatus"].Value = "进行中";
                        }
                        else if (sysTask.Status.Equals(3))//任务是否需要停止
                        {
                            task.BackgroundWorker.Stop();
                            systemTask.RowObject.Cells["RunStatus"].Value = "撤销";
                        }
                    }
                    else if (Utils.GetJObjectValue(strDown, "Type").Equals("0"))//不存在该任务
                    {
                        if (systemTask.RowObject.Index != -1)
                            dgvTack.Rows.Remove(systemTask.RowObject);
                    }
                    current++;
                    Thread.Sleep(1000);
                }
            }
            return current;
        }
        #endregion

        #region GridView

        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        /// <param name="dgv"></param>
        void InitDataGridView(DataGridView dgv)
        {

            dgv.AutoGenerateColumns = false;//是否自动创建列

            dgv.AllowUserToAddRows = false;//是否允许添加行(默认：true)

            dgv.AllowUserToDeleteRows = false;//是否允许删除行(默认：true)

            dgv.AllowUserToResizeColumns = false;//是否允许调整大小(默认：true)

            dgv.AllowUserToResizeRows = false;//是否允许调整行大小(默认：true)

            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列宽模式(当前填充)(默认：DataGridViewAutoSizeColumnsMode.None)

            dgv.BackgroundColor = System.Drawing.Color.White;//背景色(默认：ControlDark)

            dgv.BorderStyle = BorderStyle.Fixed3D;//边框样式(默认：BorderStyle.FixedSingle)

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;//单元格边框样式(默认：DataGridViewCellBorderStyle.Single)

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;//列表头样式(默认：DataGridViewHeaderBorderStyle.Single)

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;//是否允许调整列大小(默认：DataGridViewColumnHeadersHeightSizeMode.EnableResizing)

            dgv.ColumnHeadersHeight = 30;//列表头高度(默认：20)

            dgv.MultiSelect = false;//是否支持多选(默认：true)

            dgv.ReadOnly = true;//是否只读(默认：false)

            dgv.RowHeadersVisible = false;//行头是否显示(默认：true)

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选择模式(默认：DataGridViewSelectionMode.CellSelect)

        }
        /// <summary>
        /// 正在同步列表
        /// </summary>
        void AddGridViewColumns(DataGridView dgv)
        {

            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ID",
                HeaderText = "任务ID",
                Visible = false,
                Name = "ID"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "title",
                HeaderText = "任务名称",
                Name = "title"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "count",
                HeaderText = "总数",
                Name = "count"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BankName",
                HeaderText = "银行",
                Name = "BankName"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BankProjectName",
                HeaderText = "银行所属项目",
                Width = 150,
                Name = "BankProjectName"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UserName",
                HeaderText = "发起者",
                Name = "UserName"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CreateDateTime",
                HeaderText = "创建时间",
                Width = 135,
                Name = "CreateDateTime"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SuccessCount",
                HeaderText = "成功条数",
                Name = "SuccessCount"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FailureCount",
                HeaderText = "失败条数",
                Name = "FailureCount"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "RunStatus",
                HeaderText = "任务状态",
                Name = "RunStatus"
            });

        }

        #endregion

        #region 任务执行
        void AddBatch()
        {
            if (isConnected())
            {
                labNetWork.ForeColor = Color.Green;
                labNetWork.Text = "网络正常";

                string strDown = GetDownString(GetUrl("API/FxtTask.svc/TList/"));
                List<TaskToFile> ListSysTask = Utils.Deserialize<List<TaskToFile>>(Utils.GetJObjectValue(strDown, "Data"));
                if (ListSysTask != null)
                {
                    List<SystemTask> list = new List<SystemTask>();
                    foreach (var item in ListSysTask)
                    {
                        list.Add(new SystemTask()
                        {
                            Id = item.Id,
                            Status = item.Status,
                            BankName = item.BankName,
                            BankProjectName = item.BankProjectName,
                            UserName = item.UserName,
                            TaskType = item.TaskType,
                            Title = item.Title,
                            BankId = item.BankId,
                            BankProjectId = item.BankProjectId,
                            UserId = item.UserId,
                            CreateDateTime = item.CreateDateTime,
                            Url1 = item.Url1,
                            Url2 = item.Url2,
                            Url3 = item.Url3,
                            UploadFileId = item.UploadFileId,
                            FileUrl = item.FileUrl
                        });
                    }
                    int count = list.Count();

                    foreach (var item in list)
                    {
                        if (!listExists.Where(ie => ie.Equals(item.Id)).Any())
                        {
                            listExists.Add(item.Id);
                            item.RowObject = dgvTack.Rows[dgvTack.Rows.Add(item.GetArrayList().ToArray())];
                            StartRun(item);
                        }
                        Thread.Sleep(1000);
                    }

                }
            }
            else
            {
                labNetWork.ForeColor = Color.Red;
                labNetWork.Text = "网络异常";
            }
        }
        void StartRun(SystemTask systemTask)
        {
            bool isSuspend = false;
            if (systemTask.Status.Equals(2))
                isSuspend = true;
            BackgroundWorkerTimer bwTimer = new BackgroundWorkerTimer();

            #region BackgroundWorkerTimer 任务
            bwTimer.Runing += (s, e) =>
            {
                CustomEventArgs customEventArgs = ((CustomEventArgs)e);
                SystemTask doSystemTask = customEventArgs.Argument as SystemTask;
                int index = -1;
                index = doSystemTask.RowObject.Index;
                if (customEventArgs.RunComplete != null && customEventArgs.RunComplete.Cancelled)//取消
                {
                    //e.Cancel = true;
                    //break;
                }
                else if (customEventArgs.RunComplete != null && customEventArgs.RunComplete.Error != null)//错误
                {

                }
                else//成功或者继续执行
                {
                    string strGet = string.Empty;
                    if (doSystemTask.TaskType.Equals(1))
                    {
                        #region 复估
                        List<DataCollateral> list = customEventArgs.ListObject as List<DataCollateral>;
                        strGet = GetDownString(doSystemTask.Url2.Replace("{id}", list[customEventArgs.Index].Id.ToString()));
                        if (!Utils.IsNullOrEmpty(strGet) && Utils.GetJObjectValue(strGet, "Type").Equals("1"))//成功
                        {
                            doSystemTask.SuccessCount = IntNull(doSystemTask.SuccessCount);
                        }
                        else//失败
                        {
                            doSystemTask.FailureCount = IntNull(doSystemTask.FailureCount);
                            SysTaskLog sysTaskLog = new SysTaskLog()
                            {
                                TaskId = doSystemTask.Id,
                                Message = Utils.GetJObjectValue(strGet, "Message")
                            };
                            SendString(GetUrl("API/FxtTask.svc/TLC/"), new { data = Utils.Serialize(sysTaskLog) });
                        }
                        #endregion
                    }
                    else if (doSystemTask.TaskType.Equals(0))
                    {
                        #region 押品拆分
                        object[,] arrayItem = customEventArgs.ListObject as object[,];
                        ExcelHelper excelHelper = customEventArgs.ExcelHelper as ExcelHelper;
                        object objCollateral = excelHelper.TaskExcelStandardization(arrayItem,
                            customEventArgs.StandardizationHeader, customEventArgs.Index);
                        DataCollateral model = objCollateral as DataCollateral;
                        if (model != null)
                        {
                            model.UploadFileId = doSystemTask.UploadFileId;
                            model.BankId = doSystemTask.BankId;
                            model.BankProjectId = doSystemTask.BankProjectId;
                            model.Status = 0;//默认什么都没有

                            strGet = SendString(GetUrl("API/FxtTask.svc/CreateCollateral/"), new { data = Utils.Serialize(model) });
                        }
                        //!model.Id.Equals(0) 是否已存在拆分过的押品
                        if ((!Utils.IsNullOrEmpty(strGet) && Utils.GetJObjectValue(strGet, "Type").Equals("1")) ||
                            (model != null && !model.Id.Equals(0)))
                        {
                            doSystemTask.SuccessCount = IntNull(doSystemTask.SuccessCount);
                        }
                        else if ((!Utils.IsNullOrEmpty(strGet) && Utils.GetJObjectValue(strGet, "Type").Equals("0")) || model == null)
                        {
                            doSystemTask.FailureCount = IntNull(doSystemTask.FailureCount);
                            SysTaskLog sysTaskLog = new SysTaskLog()
                            {
                                TaskId = doSystemTask.Id,
                                Message = objCollateral.ToString()
                            };
                            //失败了就记录日志
                            SendString(GetUrl("API/FxtTask.svc/TLC/"), new { data = Utils.Serialize(sysTaskLog) });
                        }
                        #endregion
                    }
                    //是否下标小于0
                    if (!index.Equals(-1))
                    {
                        dgvTack.Rows[index].Cells["SuccessCount"].Value = doSystemTask.SuccessCount;
                        dgvTack.Rows[index].Cells["FailureCount"].Value = doSystemTask.FailureCount;
                    }

                    SendString(GetUrl("API/FxtTask.svc/TUSF/"), new { data = Utils.Serialize(doSystemTask) });

                    if (customEventArgs.Index == doSystemTask.Count - 1 && doSystemTask != null)
                    {
                        string strSuccess = SendString(doSystemTask.Url3, new { data = Utils.Serialize(doSystemTask) });
                        if (Utils.GetJObjectValue(strSuccess, "Type").Equals("1"))
                        {
                            if (!doSystemTask.RowObject.Index.Equals(-1))
                                doSystemTask.RowObject.Cells["RunStatus"].Value = "成功";
                            Thread.Sleep(3000);
                            //因为线程,所以异步操作
                            this.Invoke(new EventHandler((object sender, EventArgs es) =>
                            {
                                if (!index.Equals(-1))
                                {
                                    dgvTack.Rows.Remove(dgvTack.Rows[index]);
                                }
                            }));
                            
                            //if (listExists.Where(ie => ie.Equals(doSystemTask.Id)).Any())
                            //{
                            //    listExists.Remove(doSystemTask.Id);
                            //}
                            
                            //var taskControl = listTaskControl.Where(item => item.SystemTask.Id.Equals(doSystemTask.Id));
                            //if (taskControl.Any())
                            //{
                            //    listTaskControl.Remove(taskControl.FirstOrDefault());
                            //}
                        }
                    }
                }
            };
            #endregion
            bool isAdd = false;
            if (systemTask.TaskType.Equals(0) && !Utils.IsNullOrEmpty(systemTask.FileUrl))//拆分
            {
                ExcelHelper excelHelper = new ExcelHelper(GetUrl(systemTask.FileUrl, "excelParentUrl"));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Homologous/ExcelToModel.xml");
                Dictionary<int, string> listHeader = excelHelper.TaskExcelColumns(path);
                //excelHelper.TaskExcelStandardization(excelHelper.TaskReadExcel(), listHeader);
                object[,] arrayItem = excelHelper.TaskReadExcel();
                bwTimer.count = arrayItem.GetLength(0);
                systemTask.Count = arrayItem.GetLength(0);
                bwTimer.Set(systemTask, arrayItem, listHeader, excelHelper);
                isAdd = true;
            }
            else if (systemTask.TaskType.Equals(1))//复估
            {
                string strDown = GetDownString(systemTask.Url1.Replace("{id}", systemTask.UploadFileId.ToString()));
                List<DataCollateral> listData = Utils.Deserialize<List<DataCollateral>>(Utils.GetJObjectValue(strDown, "Data"));
                if (listData != null)
                {
                    bwTimer.count = listData.Count;
                    systemTask.Count = bwTimer.count;
                }
                bwTimer.Set(systemTask, listData);
                isAdd = true;
            }

            if (isAdd)
            {
                systemTask.RowObject.Cells["Count"].Value = systemTask.Count;
                bwTimer.Start(isSuspend);
                listTaskControl.Add(new TaskControl() { SystemTask = systemTask, BackgroundWorker = bwTimer });
            }
        }

        public int? IntNull(int? number)
        {
            int? resultNumber = 1;
            if (number != null)
            {
                number++;
                resultNumber = number;
            }
            else
            {
                resultNumber = 1;
            }
            return resultNumber;
        }

        public string GetDownString(string url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            //HttpClient httpClient = new HttpClient();
            try
            {
                return webClient.DownloadString(url);
                //return httpClient.GetStringAsync(url).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string SendString(string url, object obj)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.PostAsJsonAsync(url, obj).Result
                .Content.ReadAsStringAsync().Result;
            return result;
        }

        /// <summary>
        /// 获得任务相关的链接地址
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns></returns>
        private string GetUrl(string url, string key = null)
        {
            key = key == null ? "excuteUrl" : key;
            return string.Format("{0}{1}",
                ConfigurationManager.AppSettings[key].ToString(),
                url);
        }

        #endregion

        #region 检查网络状态
        //检测网络状态
        [DllImport("wininet.dll")]
        extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// 检测网络状态
        /// </summary>
        bool isConnected()
        {
            int I = 0;
            bool state = InternetGetConnectedState(out I, 0);
            return state;
        }
        #endregion
    }

    #region 任务类及任务控制类
    public class TaskControl
    {
        public SystemTask SystemTask { get; set; }
        public BackgroundWorkerTimer BackgroundWorker { get; set; }
    }
    public class SystemTask : TaskToFile
    {
        public DataGridViewRow RowObject { get; set; }
        public string RunStatus { get; set; }
        public ArrayList GetArrayList()
        {
            return new ArrayList() {
                this.Id,
                this.Title,
                this.Count,
                this.BankName,
                this.BankProjectName,
                this.UserName,
                this.CreateDateTime,
                this.SuccessCount,
                this.FailureCount,
                this.RunStatus
            };
        }
    }
    #endregion
}
