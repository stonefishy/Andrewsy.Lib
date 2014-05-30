namespace RsMgInfoSys.WinForm.Controls.SelfDefine
{
    partial class DataViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._lblStatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._btnFirst = new System.Windows.Forms.ToolStripButton();
            this._btnPrevious = new System.Windows.Forms.ToolStripButton();
            this._txtCurPage = new System.Windows.Forms.ToolStripTextBox();
            this._btnNext = new System.Windows.Forms.ToolStripButton();
            this._btnLast = new System.Windows.Forms.ToolStripButton();
            this._lblTotalPage = new System.Windows.Forms.ToolStripLabel();
            this._lblTotalRecord = new System.Windows.Forms.ToolStripLabel();
            this._lblCurRecord = new System.Windows.Forms.ToolStripLabel();
            this._listView = new Andrewsy.Lib.WinForm.Controls.ExtControl.ListViewEx();
            this._statusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _statusStrip
            // 
            this._statusStrip.BackColor = System.Drawing.Color.Transparent;
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this._progressBar,
            this._lblStatusMsg});
            this._statusStrip.Location = new System.Drawing.Point(0, 423);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(670, 22);
            this._statusStrip.TabIndex = 3;
            this._statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(34, 17);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // _progressBar
            // 
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // _lblStatusMsg
            // 
            this._lblStatusMsg.Name = "_lblStatusMsg";
            this._lblStatusMsg.Size = new System.Drawing.Size(76, 17);
            this._lblStatusMsg.Text = "正在初始化...";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._btnFirst,
            this._btnPrevious,
            this._txtCurPage,
            this._btnNext,
            this._btnLast,
            this._lblTotalPage,
            this._lblTotalRecord,
            this._lblCurRecord});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(670, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "导航：";
            // 
            // _btnFirst
            // 
            this._btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnFirst.Image = global:: Andrewsy.Lib.WinForm.Controls.Resource.moveFirst;
            this._btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnFirst.Name = "_btnFirst";
            this._btnFirst.Size = new System.Drawing.Size(23, 22);
            this._btnFirst.Text = "第一页";
            this._btnFirst.Click += new System.EventHandler(this._btnFirst_Click);
            // 
            // _btnPrevious
            // 
            this._btnPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrevious.Image = global:: Andrewsy.Lib.WinForm.Controls.Resource.movePre;
            this._btnPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnPrevious.Name = "_btnPrevious";
            this._btnPrevious.Size = new System.Drawing.Size(23, 22);
            this._btnPrevious.Text = "上一页";
            this._btnPrevious.Click += new System.EventHandler(this._btnPrevious_Click);
            // 
            // _txtCurPage
            // 
            this._txtCurPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtCurPage.Name = "_txtCurPage";
            this._txtCurPage.Size = new System.Drawing.Size(60, 25);
            this._txtCurPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtCurPage_KeyDown);
            // 
            // _btnNext
            // 
            this._btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNext.Image = global:: Andrewsy.Lib.WinForm.Controls.Resource.moveNext;
            this._btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(23, 22);
            this._btnNext.Text = "下一页";
            this._btnNext.Click += new System.EventHandler(this._btnNext_Click);
            // 
            // _btnLast
            // 
            this._btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnLast.Image = global:: Andrewsy.Lib.WinForm.Controls.Resource.moveLast;
            this._btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnLast.Name = "_btnLast";
            this._btnLast.Size = new System.Drawing.Size(23, 22);
            this._btnLast.Text = "最后一页";
            this._btnLast.Click += new System.EventHandler(this._btnLast_Click);
            // 
            // _lblTotalPage
            // 
            this._lblTotalPage.Name = "_lblTotalPage";
            this._lblTotalPage.Size = new System.Drawing.Size(58, 22);
            this._lblTotalPage.Text = "总共 0 页";
            // 
            // _lblTotalRecord
            // 
            this._lblTotalRecord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._lblTotalRecord.Name = "_lblTotalRecord";
            this._lblTotalRecord.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this._lblTotalRecord.Size = new System.Drawing.Size(78, 22);
            this._lblTotalRecord.Text = "总共 0 条";
            // 
            // _lblCurRecord
            // 
            this._lblCurRecord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._lblCurRecord.Name = "_lblCurRecord";
            this._lblCurRecord.Size = new System.Drawing.Size(70, 22);
            this._lblCurRecord.Text = "当前第 0 条";
            // 
            // _listView
            // 
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FullRowSelect = true;
            this._listView.GridLines = true;
            this._listView.Location = new System.Drawing.Point(0, 25);
            this._listView.MultiSelect = false;
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(670, 398);
            this._listView.TabIndex = 4;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.SelectedIndexChanged += new System.EventHandler(this._listView_SelectedIndexChanged);
            // 
            // DataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._listView);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "DataViewer";
            this.Size = new System.Drawing.Size(670, 445);
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.Windows.Forms.ToolStripStatusLabel _lblStatusMsg;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton _btnFirst;
        private System.Windows.Forms.ToolStripButton _btnPrevious;
        private System.Windows.Forms.ToolStripTextBox _txtCurPage;
        private System.Windows.Forms.ToolStripButton _btnNext;
        private System.Windows.Forms.ToolStripButton _btnLast;
        private System.Windows.Forms.ToolStripLabel _lblTotalRecord;
        private System.Windows.Forms.ToolStripLabel _lblCurRecord;
        private Andrewsy.Lib.WinForm.Controls.ExtControl.ListViewEx _listView;
        private System.Windows.Forms.ToolStripLabel _lblTotalPage;
    }
}
