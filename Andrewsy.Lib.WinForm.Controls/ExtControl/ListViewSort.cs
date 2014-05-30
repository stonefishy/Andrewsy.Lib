using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Andrewsy.Lib.WinForm.Controls.ExtControl
{
    public class ListViewSort:IComparer
    {
        private int _columnIndex = 0;
        private bool _bDes;

        public ListViewSort()
        {

        }

        public ListViewSort(int column, object desc)
        {
            _bDes = (bool)desc;
            _columnIndex = column;
        }

        public int Compare(object obj1, object obj2)
        {
            ListViewItem item1 = (ListViewItem)obj1;
            ListViewItem item2 = (ListViewItem)obj2;

            string item1Text = item1.SubItems[_columnIndex].Text;
            string item2Text = item2.SubItems[_columnIndex].Text;

            int tempInt = String.Compare(item1Text,item2.Text);

            if(_bDes)
                return -tempInt;
            else
                return tempInt;
        }
    }
}
