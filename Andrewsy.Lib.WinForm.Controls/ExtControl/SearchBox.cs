using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Andrewsy.Lib.WinForm.Controls.ExtControl
{
    public partial class SearchBox : UserControl
    {
        public delegate void SearchBoxEventHandler(object sender, SearchBoxEventArgs args);
        public event SearchBoxEventHandler AfterSelectedText = null;//选择下拉文本后事件
        public new event EventHandler TextChanged = null;//文本改变后事件

        private int _listDownCount = 5;//下拉个数

        public int ListDownCount
        {
            get { return _listDownCount; }
            set { _listDownCount = value; }
        }

        public SearchBox()
        {
            InitializeComponent();
            InitFunction();
        }

        //初始化
        private void InitFunction()
        {
            this._txtBox.LostFocus += new EventHandler(_txtBox_LostFocus);
            this._txtBox.TextChanged += new EventHandler(_txtBox_TextChanged);
            this.MouseCaptureChanged += new EventHandler(SearchBox_MouseCaptureChanged);
            this.Height = _txtBox.Height;
        }

        void SearchBox_MouseCaptureChanged(object sender, EventArgs e)
        {
            if(!this.Capture)
                ClearListBox();
        }

        //编辑框失去焦点时
        void _txtBox_LostFocus(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 清空ListBox
        /// </summary>
        private void ClearListBox()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is ListBox)
                    this.Controls.Remove(ctrl);
            }
            this.Height = _txtBox.Height;
        }

        /// <summary>
        /// 需要筛选排序的字符串数组
        /// </summary>
        [Browsable(true),
        Description("可供查询筛选的数组"),
        Category("SearchBox")]
        public string[] SearchItems
        {
            get;
            set;
        }

        [Browsable(true),
        Description("查询编辑框的文本内容"),
        Category("SearchBox")]
        public override string Text
        {
            get { return _txtBox.Text; }
            set { _txtBox.Text = value; }
        }

        //编辑框内容改变时
        void _txtBox_TextChanged(object sender, EventArgs e)
        {
            ShowSearchList(_txtBox.Text);

            if (TextChanged != null)
                TextChanged(this, e);
        }

        //列出过滤排序的列表
        private void ShowSearchList(string searchText)
        {
            ClearListBox();
            if (SearchItems == null || SearchItems.Length == 0)
                return;

            if (String.IsNullOrEmpty(searchText))
                return;

            //如果不存在筛选的内容
            IList<string> filterList = GetFilterSortedList(searchText, SearchItems);
            if (filterList == null || filterList.Count == 0)
                return;

            ListBox listBox = new ListBox();
            listBox.Width = this.Width;
            listBox.Location = new Point(_txtBox.Location.X, _txtBox.Location.Y + _txtBox.Height);    
            listBox.SelectedValueChanged += new EventHandler(listBox_SelectedValueChanged);

            int itemHeight = 0;
            //将筛选的内容添加到列表中
            foreach(string str in filterList)
            {
                listBox.Items.Add(str);
                itemHeight += listBox.ItemHeight;
            }

            //一次只列出10个选项
            if (itemHeight > 10 * listBox.ItemHeight)
                listBox.Height = 10 * listBox.ItemHeight;
            else
            {
                //经测试 当ListBox的Item个数小于三个后 会隐藏一个Item，此处就增加一个item的高度来显示全部Item
                if (filterList.Count < 4)
                    listBox.Height = itemHeight + listBox.ItemHeight;
                else
                    listBox.Height = itemHeight;
            }
           
            this.Height = this._txtBox.Height + listBox.Height;
            this.Controls.Add(listBox);         
        }

        //选择内容
        void listBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox  listBox  =sender as ListBox;

            _txtBox.Text = listBox.SelectedItem.ToString();

            ClearListBox();

            if (AfterSelectedText != null)
                AfterSelectedText(this, new SearchBoxEventArgs(listBox.Items.Count, this.SearchItems.Length, _txtBox.Text));
        }

        /// <summary>
        /// 获取过滤后排序的字符串
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFilterSortedList(string searchText,IList<string> searchItems)
        {
            if (SearchItems == null || SearchItems.Length == 0)
                return null;

            var result = from search in searchItems where search.StartsWith(searchText) orderby search ascending select search;
            return result.ToList<string>();
        }
    }


    public class SearchBoxEventArgs : EventArgs
    {

        private int _count = 0;//列出的个数

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        private int _totalCount = 0;//总的个数

        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }
        private string _selectedText = "";//所选内容

        public string SelectedText
        {
            get { return _selectedText; }
            set { _selectedText = value; }
        }

        public SearchBoxEventArgs()
            : base()
        {
        }

        public SearchBoxEventArgs(int count, int totalCount, string selectedText)
            : base()
        {
            this._count = count;
            this._totalCount = totalCount;
            this._selectedText = selectedText;
        }

        
    }
}
