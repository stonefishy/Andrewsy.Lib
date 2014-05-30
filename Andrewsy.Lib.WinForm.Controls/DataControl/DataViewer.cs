using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Andrewsy.Lib.AsyncDll;

namespace RsMgInfoSys.WinForm.Controls.SelfDefine
{
    public partial class DataViewer : UserControl
    {
        public event EventHandler OnInitData = null;//初始化数据事件

        private int COUNT = 20;           //一页显示个数
        private TaskAsync _taskAsync = null;    //异步处理类
        private TaskAsync _skipTaskAsync = null;
        private int _curPage = 0;               //当前第几页
        private int _totalPage = 0;             //总页数
        private DataTable _table = null;        //填充的数据

        public DataViewer()
        {
            InitializeComponent();
            InitFunction();
        }

        /// <summary>
        /// 显示的数据
        /// </summary>
        public DataTable Table
        {
            get { return _table; }
        }

        /// <summary>
        /// 一页显示个数
        /// </summary>
        public int ShowCount
        {
            get { return COUNT; }
            set { COUNT = value; }
        }

        // 填充的数据列表
        public ListView ListView
        {
            get { return _listView; }
        }

        private void InitFunction()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            _taskAsync = new TaskAsync(this);
            _taskAsync.OnTaskHandler += new TaskStatusEventHandler(_taskAsync_OnTaskHandler);
            _taskAsync.OnProcessed += new TaskStatusEventHandler(_taskAsync_OnProcessed);
            _taskAsync.OnProcessing += new TaskStatusEventHandler(_taskAsync_OnProcessing);
            _taskAsync.OnProcessError += new TaskStatusEventHandler(_taskAsync_OnProcessError);

            _skipTaskAsync = new TaskAsync(this);
            _skipTaskAsync.OnTaskHandler += new TaskStatusEventHandler(_skipTaskAsync_OnTaskHandler);
            _skipTaskAsync.OnProcessing += new TaskStatusEventHandler(_skipTaskAsync_OnProcessing);
            _skipTaskAsync.OnProcessStarted += new TaskStatusEventHandler(_skipTaskAsync_OnProcessStarted);
            _skipTaskAsync.OnProcessed += new TaskStatusEventHandler(_skipTaskAsync_OnProcessed);
        }

        /// <summary>
        /// 将数据填充进去
        /// </summary>
        /// <param name="table"></param>
        public void FillData(DataTable table)
        {
            if (table == null)
                return;

            _table = table;
            _taskAsync.AbortTask();
            _listView.Clear();
            _taskAsync.StartTask(table);
        }

        void _skipTaskAsync_OnProcessed(object sender, TaskStatusArgs args)
        {
            SetStatusProgress("就绪", 0);
        }

        void _skipTaskAsync_OnProcessStarted(object sender, TaskStatusArgs args)
        {
            _txtCurPage.Text = _curPage.ToString();
        }

        void _skipTaskAsync_OnProcessing(object sender, TaskStatusArgs args)
        {
            DataRow row = args.Tag as DataRow;

            if (row == null)
                return;

            int index = Convert.ToInt32(args.StatusMsg);
            int xuHao = index + (_curPage - 1) * COUNT;

            //将数据添加到列表
            AddRecordItem(row, xuHao);
        }

        /// <summary>
        /// 将一条记录添加列表视图
        /// </summary>
        /// <param name="row">数据</param>
        private void AddRecordItem(DataRow row, int xuHao)
        {
            ListViewItem item = new ListViewItem() { Tag = row };

            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();

            subItem.Text = xuHao.ToString();
            item.SubItems.Add(subItem);

            int colCount = row.Table.Columns.Count;

            for (int i = 0; i < colCount; i++)
            {
                subItem = new ListViewItem.ListViewSubItem() { Text = row[i].ToString() };
                item.SubItems.Add(subItem);
            }

            _listView.Items.Add(item);
        }

        void _skipTaskAsync_OnTaskHandler(object sender, TaskStatusArgs args)
        {

            int curPage = Convert.ToInt32(args.Tag);
            DataRow[] rows = _table.Select();
            var result = (from row in rows select row).Skip((_curPage - 1) * COUNT).Take(COUNT);

            int index = 0;
            int count = result.Count();

            foreach (var row in result)
            {
                ++index;
                //数据更新
                _skipTaskAsync.SendUpdateMessage("", index.ToString(), row);
                SetStatusProgress("数据加载中", index, count);
            }

            SetStatusProgress("就绪", 0, count);
            args.Tag = count;           
        }

        void _taskAsync_OnProcessed(object sender, TaskStatusArgs args)
        {
            DataTable  table = args.Tag as DataTable;

            if (table == null)
                return;

            int totalCount =  table.Rows.Count;  //总条数
            _totalPage = totalCount / COUNT + 1;    //总页数

            _lblTotalPage.Text = String.Format("总页数: {0} 页", _totalPage);
            _lblTotalRecord.Text = String.Format("总条数: {0} 条", totalCount);

            _curPage = 1;
            _skipTaskAsync.StartTask(_curPage);
        }

        void _taskAsync_OnProcessing(object sender, TaskStatusArgs args)
        {
            string strMsg = args.Tag.ToString();
            SetStatusProgress(strMsg, 50, 100);
            InitListView();
            SetStatusProgress(strMsg, 80, 100);
        }

        void _taskAsync_OnTaskHandler(object sender, TaskStatusArgs args)
        {
            DataTable table = args.Tag as DataTable;

            if (table == null)
                return;

            SetStatusProgress("正在初始化...", 30, 100);
            _taskAsync.SendUpdateMessage("正在初始化...");

            //后台其他初始化
            if (OnInitData != null)
                OnInitData(this, new EventArgs());

            SetStatusProgress("就绪", 0, 100);

            args.Tag = table;
        }


        void _taskAsync_OnProcessError(object sender, TaskStatusArgs args)
        {
            MessageBox.Show(args.ErrorMsg);
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitListView()
        {
            ColumnHeader header = new ColumnHeader();
            header.Width = 0;
            _listView.Columns.Add(header);

            header = new ColumnHeader();
            header.Text = "序号";
            header.TextAlign = HorizontalAlignment.Center;
            _listView.Columns.Add(header);

            foreach (DataColumn col in _table.Columns)
            {
                header = new ColumnHeader();
                header.Text = col.ColumnName;
                header.TextAlign = HorizontalAlignment.Center;
                _listView.Columns.Add(header);
            }
        }

        /// <summary>
        /// 设置状态进度信息
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        private void SetStatusProgress(string strMsg, int value, int maxValue = 100)
        {
            if (this._statusStrip.InvokeRequired)
            {
                this._statusStrip.Invoke(new Action(() =>
                {
                    _lblStatusMsg.Text = strMsg;
                    _progressBar.Maximum = maxValue;
                    _progressBar.Value = value;
                }));
            }
            else
            {
                _lblStatusMsg.Text = strMsg;
                _progressBar.Maximum = maxValue;
                _progressBar.Value = value;
            }
        }

        //跳转查询
        private void NavigateSearch(int navgToPage)
        {
            _curPage = navgToPage;
            _skipTaskAsync.AbortTask();

            _listView.Items.Clear();
            _skipTaskAsync.StartTask(_curPage);
        }

        //检查是否是第一页或最后一页
        private bool CheckPreNextPage(bool bNext)
        {
            string strMsg = String.Empty;

            if (_curPage == _totalPage && bNext)
            {
                strMsg = "已经是最后一页了";
                SetStatusProgress(strMsg, 0);
                return false;
            }
            else if (_curPage == 1 && !bNext)
            {
                strMsg = "已经是第一页了";
                SetStatusProgress(strMsg, 0);
                return false;
            }

            strMsg = "就绪";
            SetStatusProgress(strMsg, 0);

            return true;
        }

        //第一页
        private void _btnFirst_Click(object sender, EventArgs e)
        {
            if (!CheckPreNextPage(false)) return;

            _curPage = 1;
            NavigateSearch(_curPage);
        }

        //上一页
        private void _btnPrevious_Click(object sender, EventArgs e)
        {
            if (!CheckPreNextPage(false)) return;

            _curPage = --_curPage < 1 ? 1 : _curPage;
            NavigateSearch(_curPage);
        }

        //下一页
        private void _btnNext_Click(object sender, EventArgs e)
        {
            if (!CheckPreNextPage(true)) return;

            _curPage = ++_curPage > _totalPage ? _totalPage : _curPage;
            NavigateSearch(_curPage);
        }

        //最后一页
        private void _btnLast_Click(object sender, EventArgs e)
        {
            if (!CheckPreNextPage(true)) return;

            _curPage = _totalPage;
            NavigateSearch(_curPage);
        }

        private void _listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_listView.SelectedIndices.Count > 0)
            {
                int index = _listView.SelectedIndices[0] + 1;
                int curRecord = (_curPage - 1) * COUNT + index;
                _lblCurRecord.Text = String.Format("当前第: {0} 条", curRecord);
            }
        }

        private void _txtCurPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int page = Convert.ToInt32(_txtCurPage.Text);

                    if (page <= 0 || page > _totalPage)
                    {
                        MessageBox.Show("提示", "输入的页数范围不正确!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _curPage = page;
                    NavigateSearch(_curPage);
                }
                catch
                {
                    MessageBox.Show("提示", "输入的页数格式不正确!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
        }
    }
}
