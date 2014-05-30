namespace Andrewsy.Lib.WinForm.Controls.ExtControl
{
    partial class SearchBox
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
            this._txtBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _txtBox
            // 
            this._txtBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._txtBox.Location = new System.Drawing.Point(0, 0);
            this._txtBox.Name = "_txtBox";
            this._txtBox.Size = new System.Drawing.Size(190, 21);
            this._txtBox.TabIndex = 0;
            // 
            // SearchBox
            // 
            this.Controls.Add(this._txtBox);
            this.Name = "SearchBox";
            this.Size = new System.Drawing.Size(190, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtBox;
    }
}
